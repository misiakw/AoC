from input import *
import sys

inp = RawInput(sys.argv[1])

floor = 0
firstBasement = -1
ctr = 1
for ch in inp.raw:
    if ch == '(':
        floor = floor + 1
    if ch == ')':
        floor = floor - 1
    if floor < 0 and firstBasement < 0:
        firstBasement = ctr
    ctr = ctr + 1

print("part1: "+str(floor))
print("part1: "+str(firstBasement))