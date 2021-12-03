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
        self.width = len(lines[0].strip())
        self.height = len(lines)
        self.map = {}
        for y in range(0, self.height):
            for x in range(0, self.width):
                self.map[(x, y)] = lines[y][x]        
    
    def copy(self):
        result = Map("")
        result.height = self.height
        result.width = self.width
        result.map = self.map.copy()
        return result