from input import RawInput
import sys

def transformToOnp(cmd, precd):
    result = []
    chrs = []
    maxChrs = 0
    for op in cmd.replace("(", "( ").replace(")", " )").split(" "):
        if op.isnumeric():
            result.append(op)
        if op == "(":
            maxChrs +=1
            chrs.append(op)            
        if op == ")":
            maxChrs -=1
            while len(chrs) > maxChrs:
                ch = chrs.pop()
                if ch == "(":
                    break
                result.append(ch)
        if op in ["+","*"]:
            if len(chrs) > 0 and chrs[-1] in precd:
                ch = chrs[-1]
                if precd[ch] >= precd[op]:
                    result.append(chrs.pop())
                if len(chrs) > 0 and precd.get(chrs[-1], -1) == precd[op]:
                    result.append(chrs.pop())     
                chrs.append(op)
            else:
                chrs.append(op)
        
    while len(chrs) > 0:
        result.append(chrs.pop())
    return result

def calculate(cmds)->int:
    result = []
    for ch in cmds:
        result.append(int(ch) if ch.isnumeric() else result.pop()+result.pop() if ch  == "+" else result.pop()*result.pop())
    return result[0]

inp = RawInput(sys.argv[1])

precd ={"+": 0, "*": 0}
p1 = sum([calculate(transformToOnp(x, precd)) for x in inp.getParts()])
print("part1", p1)

precd ={"+": 1, "*": 0}
p2 = sum([calculate(transformToOnp(x, precd)) for x in inp.getParts()])
print("part2", p2)
