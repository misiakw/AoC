from input import Input
import sys

def getLoopCount(key):
    v = 1
    l = 0
    while v != key:
        v = (v*7)%20201227
        l+=1
    return l

inp = Input(sys.argv[1], int)

loop1 = getLoopCount(inp.getInput()[0])
print("part1", pow(inp.getInput()[1], loop1, 20201227))