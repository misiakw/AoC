from input import Input
import sys

class Command:
    def __init__(self, line):
        parts = line.split(" ")
        self.times = 0
        self.val = int(parts[1])
        self.cmd = parts[0]

    #nextCmd, accumlator
    def run(self, iterator, accumulator) -> (int, int):
        self.times += 1
        if self.cmd == "nop":
            return (iterator+1, accumulator)
        if self.cmd == "acc":
            return (iterator+1, accumulator+self.val)
        if self.cmd == "jmp":
            return (iterator+self.val, accumulator)

def runProg(prog) -> (bool, int):
    acc = 0
    iterator = 0
    while iterator < len(prog):
        cmd = prog[iterator]
        if cmd.times == 0:
            result = cmd.run(iterator, acc)
            acc =  result[1]
            iterator = result[0]
        else:
            return (False, acc)
    return (True, acc)
def runMutations(prog):
    for i in range(0, len(prog)):
        for j in range(0, len(prog)):
            prog[j].times = 0

        if prog[i].cmd == "nop":
            prog[i].cmd = "jmp"
            result = runProg(prog)
            if result[0]:
                return result
            prog[i].cmd = "nop"

        if prog[i].cmd == "jmp":
            prog[i].cmd = "nop"
            result = runProg(prog)
            if result[0]:
                return result
            prog[i].cmd = "jmp"

inp = Input(sys.argv[1], lambda x : Command(x))

p1 = runProg(inp.getInput())
print("part1: ", p1[1])
p2 = runMutations(inp.getInput())
print("part1: ", p2[1])


