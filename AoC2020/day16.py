from input import RawInput
import sys
import numpy

class Ticket:
    def __init__(self, data):
        self.values = [int(x) for x in data.split(",")]
        self.matches = {}
    def __repr__(self):
        return "Ticket["+",".join([str(x) for x in self.values])+"]"
    def getMatchingRules(self, rules):
        for x in self.values:
            self.matches[x] = [r for r in rules if r.matchRule(x)]
        return self.matches
            
class Rule:
    def __init__(self, data):
        self.name = data.split(":")[0]
        self.ranges = []
        for rng in data.split(":")[1][1:].split(" or "):
            parts = rng.split("-")
            self.ranges.append((int(parts[0]), int(parts[1])))
    def __repr__(self):
        return "Rule("+self.name+")"
    def matchRule(self, value):
        for r in self.ranges:
            if value >= r[0] and value <= r[1]:
                return True
        return False

#start gathering input
inp = RawInput(sys.argv[1])
i = 0
rules = []
while inp.parts[i].strip() != "":
    rules.append(Rule(inp.parts[i]))
    i+=1
i+=2
yourTicket = Ticket(inp.parts[i].strip())
i+=3
otherTickets = [Ticket(inp.parts[x].strip()) for x in range(i, len(inp.parts))]
#finish gathering inputs

#part1
p1 = 0
invalidTikets = []
for t in otherTickets:
    for k in t.getMatchingRules(rules).items():
        if len(k[1]) == 0:
            p1 += k[0]
            invalidTikets.append(t)
print("part1", p1)
#finish part 1

[otherTickets.remove(t) for t in invalidTikets]

fields = {}
yourTicket.getMatchingRules(rules)
for x in range(0, len(yourTicket.values)):
    fields[x] = yourTicket.matches[yourTicket.values[x]]

#get all rules matching ticket fields
for t in otherTickets:
    for x in range(0, len(t.values)):
        others = t.matches[t.values[x]]
        toRemove = [f for f in fields[x] if f not in others]
        [fields[x].remove(f) for f in toRemove]
            

#reduce keys that are rules to be the only possible rule for given field
keysToReduce = [key for key in fields if len(fields[key]) > 1]
reducedRules = [fields[key][0] for key in fields if len(fields[key]) == 1]
while len(keysToReduce) > 0:
    for key in keysToReduce:
        [fields[key].remove(red) for red in reducedRules if  red in fields[key]]
               
    keysToReduce = [key for key in fields if len(fields[key]) > 1]
    reducedRules = [fields[key][0] for key in fields if len(fields[key]) == 1]

p2 = 1
for x in [yourTicket.values[i] for i in fields if fields[i][0].name.split(" ")[0] == "departure"]:
    p2 *= x
        
print("part2", p2)