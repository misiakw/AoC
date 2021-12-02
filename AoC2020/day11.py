from input import RawInput
import sys

class Chair:
    def __init__(self, x, y, chairs):
        self.chairs = chairs
        self.x = x
        self.y = y
        self.state = "L"
        self.next = "L"
    def __repr__(self):
        return self.state
    def calcNext(self):
        points = [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)]
        taken = 0
        for p in points:
            x = self.x + p[0]
            y = self.y + p[1]
            taken += (1 if self.chairs.get((x, y)) is not None and self.chairs[x,y].state == "#" else 0)
        if self.state == "L" and taken == 0:
            self.next = "#"
            return True
        if self.state == "#" and taken > 3:
            self.next = "L"
            return True
        return False
    def calcNext2(self):
        taken = 0
        mods = [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)]
        for mod in mods:
            x = self.x+mod[0]
            y = self.y+mod[1]
            while self.chairs.get((x, y)) is None:
                if x < 0 or y < 0:
                    break
                if x > room.width or y > room.height:
                    break
                x += mod[0]
                y += mod[1]
            taken += (1 if self.chairs.get((x, y)) is not None and self.chairs[x, y].state == "#" else 0)
        if self.state == "L" and taken == 0:
            self.next = "#"
            return True
        if self.state == "#" and taken >= 5:
            self.next = "L"
            return True
        return False
class Room:
    def __init__(self, lines):
        self.width = len(lines[0])
        self.height = len(lines)
        self.chairs = {}
        self.chairList = []
        for y in range(0, self.height):
            for x in range(0, self.width):
                if lines[y][x] == "L":
                    chair = Chair(x, y, self.chairs)
                    self.chairs[(x, y)] = chair
                    self.chairList.append(chair)
    def doRound(self):
        wasChange = False
        for ch in self.chairList:
            wasChange |= ch.calcNext()
        for ch in self.chairList:
            ch.state = ch.next
        return wasChange 
    def doRound2(self):
        wasChange = False
        for ch in self.chairList:
            wasChange |= ch.calcNext2()
        for ch in self.chairList:
            ch.state = ch.next
        return wasChange 
    def getOcupiedCount(self):
        return len([ch for ch in self.chairList if ch.state == "#"])
    def __repr__(self):
        result = ""
        for y in range(0, self.height):
            for x in range(0, self.width):
                result += self[x,y]
            result += "\n"
        return result[:-1]
    def __getitem__(self, key):
        return (self.chairs[key].state if self.chairs.get(key) is not None else ".")

inp = RawInput(sys.argv[1])
room = Room(inp.getParts())

wasChange = True
while wasChange:
    wasChange = room.doRound()
print("part1: ", room.getOcupiedCount())

room = Room(inp.getParts())
wasChange = True
while wasChange:
    wasChange = room.doRound2()
print("part2: ", room.getOcupiedCount())