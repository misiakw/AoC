from input import RawInput
import sys
import numpy as np

class Wire:
    def __init__(self, name):
        self.outputs = []
        self.name = name
        self.value = None
    
    def addOutput(self, func):
        self.outputs.append(func)
        if self.value is not None:
            func(self.value)
    
    def transmit(self, value):
        self.value = value
        for output in self.outputs:
            output(value)
    
    def __repr__(self):
        return "["+self.name+"]("+str(self.value)+")"
    
class PassGate:
    def __init__(self, cmd, output):
        self.output = output
        self.value = None
        
        if cmd.isnumeric():
            self.setVal(np.uint16(cmd))
        else:
            wire = getWire(cmd)
            wire.addOutput(lambda  x: self.setVal(x))

    def setVal(self, value):
        self.value = np.uint16(value)
        self.transmit()

    def transmit(self):
        if self.value is None:
            return
        self.output.transmit(self.value)

class NegGate(PassGate):
    def setVal(self, value):
        self.value = ~value
        self.transmit()

class Gate:
    def __init__(self, inp1, inp2, output, operation):
        self.output = output
        self.operation = operation
        self.left = None
        self.right = None

        if inp1.isnumeric():
            self.setLeft(np.uint16(inp1))
        else:
            wire = getWire(inp1)
            wire.addOutput(lambda  x: self.setLeft(x))
        
        if inp2.isnumeric():
            self.setRight(np.uint16(inp2))
        else:
            wire = getWire(inp2)
            wire.addOutput(lambda  x: self.setRight(x))


    def setLeft(self, value):
        self.left = np.uint16(value)
        self.transmit()
    
    def setRight(self, value):
        self.right = np.uint16(value)
        self.transmit()

    def transmit(self):
        if self.left is None or self.right is None:
            return
        self.output.transmit(self.operation(self.left, self.right))

def getWire(name) -> Wire:
    if wires.get(name) is None:
        wires[name] = Wire(name)
    return wires[name]

inp = RawInput(sys.argv[1])

wires = {}
for cmd in inp.getLines():
    sides = cmd.split("->")
    wire = getWire(sides[1].strip())
    
    parts = sides[0].strip().split()
    if len(parts) == 1:
        PassGate(parts[0].strip(), wire)
    if len(parts) == 2:
        NegGate(parts[1].strip(), wire)
    if len(parts) == 3:
        if parts[1] == "AND":
            Gate(parts[0], parts[2], wire, lambda l, r: l&r)
        if parts[1] == "OR":
            Gate(parts[0], parts[2], wire, lambda l, r: l|r)



print(wires)