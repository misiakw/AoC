import sys
class Point:
    def __init__(self, points):
        parts = points.split(',')
        self.x = int(parts[0])
        self.y = int(parts[1])

    def __str__(self):
        return str(self.x)+"x"+str(self.y)

class Map:
    def __init__(self, rawInput):
        lines = rawInput.strip().splitlines()
        self.raw = rawInput
        self.width = len(lines[0])
        self.height = len(lines)
        self.map = self.getClean()
    
    def getClean(self):
        lines = self.raw.strip().splitlines()
        result = {}
        for y in range(0, self.height):
            for x in range(0, self.width):
                result[(x, y)] = lines[y][x]
        return result
    
    def copy(self):
        result = Map(self.raw)
        result.height = self.height
        result.width = self.width
        result.map = self.map.copy()
        return result

    def expandCopyRight(self):
        lines = self.raw.strip().splitlines()
        for y in range(0, len(lines)):
            for x in range(0, len(lines[y])):
                self.map[(x+self.width, y)] = lines[y][x]
        self.width += len(lines[y])

    def __repr__(self):
        line = ""
        for y in range(0, self.height):
            for x in range(0, self.width):
                line += self[x, y]
            line += "\n"
        return line[:-1]
    
    def __getitem__(self, key):
        return self.map.get(key)
            
    def __setitem__(self, key, value):
        self.map[key] = value