from input import Input
import sys

def isInPreamble(data, search) -> bool:
    for i in range(0, len(data)):
        for j in range(i+1, len(data)):
            if search == data[i]+data[j]:
                return True
    return False

def getRangeToSum(data, start, search):
    for i in range(start, len(data)):
        if sum(data[start:i]) == search:
            return data[start:i]
        if sum(data[start:i]) > search:
            return []
    return []

inp = Input(sys.argv[1], int)
data = inp.getInput()

preambleLen = data[0]
data = data[1:]

num = 0
for i in range(0, len(data) - preambleLen):
    preamble = data[i:i+preambleLen]
    num = data[i+preambleLen]
    if not isInPreamble(preamble, num):
        break

print("part1:", num)

for i in range(0, len(data)-preambleLen):
    group = getRangeToSum(data, i, num)
    if len(group) > 0:
        break
print("part2", min(group) + max(group))