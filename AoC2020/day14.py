from input import RawInput
import sys

inp = RawInput(sys.argv[1])

class Mask:
    def __init__(self, val):
        self.mask = [x for x in val]
        self.andMask = int(val.replace('X', '1'), base=2)
        self.orMask = int(val.replace('X', '0'), base=2)
    def apply(self, val):
        result = self.andMask & val
        result = self.orMask | result
        return result
    def getMappedAddresses(self, val):
        bval = [x for x in format(val, "036b")]
        for i in range(0, len(self.mask)):
            if self.mask[i] == '1':
                bval[i] = '1'
            if self.mask[i] == 'X':
                bval[i] = "X"
        result = [int(x, base=2) for x in self.permutate("".join(bval))]
        return result
    def permutate(self, val)->[str]:
        if len(val) == 1:
            return ["0", "1"] if val[0] == "X" else [val]
        prefix = val[:-1]
        ch = val[-1]
        result =[]
        for x in self.permutate(prefix):
            if ch == "X":
                result.append(x+"0")
                result.append(x+"1")
            else:
                result.append(x+ch)
        return result
mask = Mask("0")
mem = {}
mem2 = {}
for line in inp.getParts():
    cmd = line.split(" = ")
    if cmd[0] == "mask":
        mask = Mask(cmd[1])
    else:
        mem[int(cmd[0][4:-1])] = mask.apply(int(cmd[1]))
        for addr in mask.getMappedAddresses(int(cmd[0][4:-1])):
            mem2[addr] = int(cmd[1])
suma = 0
for x in mem:
    suma += mem[x]
print("part1:", suma)
suma = 0
for x in mem2:
    suma += mem2[x]
print("part2:", suma)