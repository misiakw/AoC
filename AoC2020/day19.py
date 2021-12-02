from input import RawInput
import sys

inp = RawInput(sys.argv[1])

class Rule():
    def __init__(self, num, rules):
        self.rules = []
        self.value = []
        self.canSolve = False
        self.num = num
        self.allRules = rules
    def introduce(self, cmd):
        for subset in [x.split(" ") for x in cmd.split(" | ")]:
            if len(subset) == 1 and not subset[0].isnumeric():
                self.value = [subset[0].replace("\"", "")]
                self.canSolve = True
            else:
                self.rules.append([getRule(int(x), self.allRules) for x in subset])
    def solve(self):
        if self.canSolve:
            return
        for subset in self.rules:
            for r in subset:
                if  not r.canSolve:
                    return
        self.getValue()
        self.canSolve = True
    def getValue(self)->int:
        if len(self.value) > 0:
            return self.value
        for subset in self.rules:
            value = subset[0].getValue()
            for i in range(1, len(subset)):
                tmp = []
                for v1 in value:
                    [tmp.append(v1+v2) for v2 in subset[i].getValue()]
                value = tmp
            [self.value.append(x) for x in value if x not in self.value]
        return self.value
    def isMatch(self, msg):
        if self.canSolve:
            return msg in self.value
        #sizes = [(min([len(x) for x in rule.value]), max([len(x) for x in rule.value]))for rule in self.rules[0]]
        minSize = 24 #magic number because of data i know, to be implemented
        maxSize = 24 #magic number because of data i know, to be implemented
        if len(msg) < minSize or len(msg) > maxSize:
            return False
        p1 = msg[:8]
        p2 = msg[8:]
        return p1 in self.rules[0][0].value and p2 in self.rules[0][1].value
        
def getRule(number, rules)->Rule:
    if rules.get(number, None) == None:
        rules[number] = Rule(number, rules)
    return rules[number]

def isMatchPart2(value)->bool:
    sets = []
    i = 0
    value42 = rules[42].value
    value31 = rules[31].value
    while i < len(value):
        tmp = value[i:i+8]
        tmpSet = []
        if tmp in value42:
            tmpSet.append(42)
        if tmp in value31:
            tmpSet.append(31)
        if len(tmpSet) == 0:
            return False
        sets.append(tmpSet)
        i+=8

    split = 0
    while split < len(sets)-1 and 31 in sets[-1*(split+1)]:
        split += 1
    if len(sets) < 2*split + 1 or split == 0:
        return False
    set8 = sets[:-2*split]
    set11 = sets[-2*split:]

    for x in set8:
        if 42 not in x:
            return False
    for i in range(0, int(len(set11)/2)):
        if 42 not in set11[i] or 31 not in set11[-1*(i+1)]:
            return False
    return True

rules = {}
setRules = True
messages = []
for line in inp.getParts():
    if line == "":
        setRules = False
        continue
    if setRules:
        number = int(line.split(":")[0])
        getRule(number, rules).introduce(line.split(":")[1].strip())
    else:
        messages.append(line)

allRules = [rules[x] for x in rules]
unsolved = [x for x in allRules if not x.canSolve]
while len(unsolved) > 1:
    print("to solve: "+str(len(unsolved))+"\\"+str(len(allRules)))
    [x.solve() for x in unsolved]
    unsolved = [x for x in allRules if not x.canSolve]

results = [(rules[0].isMatch(x), x) for x in messages]
print("part1", len([x for x in results if x[0]]))

results2 = [(isMatchPart2(x), x) for x in messages]
print("part2", len([x for x in results2 if x[0]]))