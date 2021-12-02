from input import RawInput
import sys

def buildAddressSet(arr):
    if len(arr) == 1:
            return [str(x) for x in range(arr[0][0], arr[0][1]+1)]
    tail = buildAddressSet(arr[:-1])
    result = []
    for r in range(arr[-1][0], arr[-1][1]+1):
        for t in tail:
            result.append(t+","+str(r))
    return result

class Cube:
    def __init__(self, poz, state, mesh):
        self.poz = [int(x) for x in poz]
        self.active = state
        self.mesh = mesh
        self.futureState = state
    def getNeigbours(self):
        ranges = [[self.poz[dim]-1, self.poz[dim]+1] for dim in range(0, len(self.poz))]
        addr = buildAddressSet(ranges)
        result = []
        for a in addr:
            single = [int(x) for x in a.split(",")]
            if not single == self.poz:
                result.append(self.mesh[single])
        return result
    def calculate(self):
        activeAround = sum([1 for nbr in self.getNeigbours() if nbr.active])
        if self.active and (activeAround < 2 or activeAround > 3):
            self.futureState = False
            return True
        if (not self.active) and activeAround == 3:
            self.futureState = True
            return True
        return False
    def persist(self):
        self.active = self.futureState
    def __repr__(self):
        return "#" if self.active else "."


class Mesh:
    def __init__(self, lines, dimentions):
        self.mesh = {}
        self.dimentions = dimentions
        addr = [0 for x in range(0, dimentions)]
        for y in range(0, len(lines)):
            addr[1] = y
            for x in range(0, len(lines[0])):
                addr[0] = x
                self[addr] = Cube(addr, lines[y][x] == "#", self)
    def __setitem__(self, key, value):
        k = ",".join([str(x) for x in key])
        self.mesh[k] = value
    def __getitem__(self, key)->Cube:        
        k = ",".join([str(x) for x in key])
        if self.mesh.get(k, None) is None:
            self.mesh[k] = Cube(key, False, self)
        return self.mesh[k]
    def doRound(self):
        keys = [k.split(",") for k in self.mesh]
        ranges = []
        for dim in range(0, self.dimentions):
            ints = [int(x[dim]) for x in keys]
            ranges.append((min(ints)-1, max(ints)+1))
        consideredAddresss = []
        for addrStr in buildAddressSet(ranges):
            addr = [int(x) for x in addrStr.split(",")]
            consideredAddresss.append(addr)
            self[addr].calculate()
        [self[addr].persist() for addr in consideredAddresss]
        self.erase()
    def erase(self):
        toDelete = [key for key in self.mesh if not self.mesh[key].active]
        for key in toDelete:
            del self.mesh[key]
    def countActive(self)->int:
        return sum([1 for key in self.mesh if self.mesh[key].active])

inp = RawInput(sys.argv[1])

mesh = Mesh(inp.getParts(), 3)
for i in range(0, 6):
    mesh.doRound()
print("part1", mesh.countActive())

mesh = Mesh(inp.getParts(), 4)
for i in range(0, 6):
    mesh.doRound()
print("part2", mesh.countActive())