from input import Input
import sys

inp = Input(sys.argv[1], int, ",")

init = inp.getInput()
def getlastValueForStep(init, limit):
    lastSeen = {init[x]:x+1 for x in range(0, len(init)-1)}
    toSay = init[-1]
    for r in range(len(init), limit):
        prevVal = lastSeen.get(toSay, 0)
        lastSeen[toSay] = r    
        toSay = r-prevVal if prevVal > 0 else 0
    return toSay

print("part1",getlastValueForStep(inp.getInput(), 2020))
print("part2",getlastValueForStep(inp.getInput(), 30000000))