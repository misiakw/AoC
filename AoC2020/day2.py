from input import Input
import sys

class Entity:
    def __init__(self, ent):
        parts = ent.split()
        times = parts[0].split('-')
        self.minimum = int(times[0])
        self.maximum = int(times[1])
        self.ch = parts[1][:-1]
        self.passwd = parts[2]

    def isValid(self):
        occur = self.passwd.count(self.ch)
        if occur >= self.minimum and occur <= self.maximum:
            return 1
        return 0
    
    def isValid2(self):
        ch1 = self.passwd[self.minimum-1]
        ch2 = self.passwd[self.maximum-1]
        if ch1 == ch2:
            return 0
        if ch1 == self.ch or ch2 == self.ch:
            return 1
        return 0

inp = Input(sys.argv[1], lambda x: Entity(x))

valid = 0
valid2 = 0
for ent in inp.getInput():
    if ent.isValid():
        valid += 1
    if ent.isValid2():
        valid2 += 1

print("part1", valid)
print("part1", valid2)