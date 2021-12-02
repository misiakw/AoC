from input import Input
import sys

class Seat:
    def __init__(self, num):
        num = num.replace("F", "0")
        num = num.replace("B", "1")
        num = num.replace("L", "0")
        num = num.replace("R", "1")
        self.row = int(num[0:7], 2)
        self.col = int(num[7:], 2)

    def uid(self) -> int:
        return self.row * 8 + self.col

inp = Input(sys.argv[1], lambda x: Seat(x))

highest = 0
lowest = 999999999
uids = []
for s in inp.getInput():
    uids.append(s.uid())
    if s.uid() > highest:
        highest = s.uid()
    if s.uid() < lowest:
        lowest = s.uid()

uids.sort()

for x in range(lowest, highest):
    if x not in uids:
        missing = x

print("part1", highest)
print("part2", missing)