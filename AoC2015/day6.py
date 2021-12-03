from input import Input, numpy
from myTypes import Point
import sys
from enum import IntEnum

class Action(IntEnum):
    OFF=0
    ON=1
    TOGGLE = 2


class LightAction:
    def __init__(self, line):
        self.line = line
        parts = line.split(' ')

        if parts[0] == "toggle":
            self.action = Action.TOGGLE
            self.start = Point(parts[1])
            self.end = Point(parts[3])
        else:
            if parts[1] == "on":
                self.action = Action.ON
            else:
                self.action = Action.OFF
            self.start = Point(parts[2])
            self.end = Point(parts[4])
    
    def call(self, arr1, arr2):
        for x in range(self.start.x, self.end.x+1):
            for y in range(self.start.y, self.end.y+1):
                if self.action == Action.ON:
                    arr1[x, y] = 1
                    arr2[x, y] += 1
                if self.action == Action.OFF:
                    arr1[x, y] = 0
                    arr2[x, y] -= 1
                    if arr2[x, y] < 0:
                        arr2[x, y] = 0
                if self.action == Action.TOGGLE:
                    arr2[x, y] += 2
                    if arr1[x, y] == 0:
                        arr1[x, y] = 1
                    else:
                        arr1[x, y] = 0


inp = Input(sys.argv[1], lambda x: LightAction(x))

lights = numpy.zeros((1000, 1000), dtype=int)
lights2 = numpy.zeros((1000, 1000), dtype=int)

for action in inp.prepared:
    action.call(lights, lights2)

print("part1: ", sum(sum(lights)))
print("part2: ", sum(sum(lights2)))