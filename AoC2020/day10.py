from input import Input
import sys

def permuteGroup(group):
    result = 1
    if len(group) > 2:
        result = listPermutations([group[0]], group[1:])
    return result

def listPermutations(preamble, rest):
    if len(rest) == 0:
        return 1
    result = 0
    for i in range(0, len(rest)):
        if rest[i] - preamble[-1] > 3:
            return result
        tmp = preamble.copy()
        tmp.append(rest[i])
        result += listPermutations(tmp, rest[i+1:])
    return result

inp = Input(sys.argv[1], int)
chargers = sorted(inp.getInput())

currvolt = 0
difs = [0, 0, 0, 0]
for i in range(0, len(chargers)):
    dif = chargers[i] - currvolt
    if dif > 0:        
        difs[dif] += 1
        currvolt = chargers[i]

print("task1:", difs[1]*(difs[3]+1))

groups = []
currGroup = []
for i in range(0, len(chargers)-2):
    currGroup.append(chargers[i])
    if chargers[i+2] - chargers[i] > 3:
        currGroup.append(chargers[i+1])
        groups.append(currGroup)
        currGroup = []
        i += 1

for charger in chargers[-2:]:
    currGroup.append(charger)
groups.append(currGroup)

possibles = 1
for group in filter(lambda x: len(x) > 2, groups):
    possibles *= permuteGroup(group)

print("task2:", possibles)