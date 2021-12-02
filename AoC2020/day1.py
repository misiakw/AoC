from input import Input
import sys

inp = Input(sys.argv[1], int)

def SearchSum(source, collection, res, amount):
    if len(collection) == amount:
        if sum(collection) == res:
            return collection
        else:
            return []
    
    x = 0
    while x < len(source):
        if sum(collection) + source[x] <= res:
            newCol = collection.copy()
            newCol.append(source[x])
            resp = SearchSum(source[x+1:], newCol, res, amount)
            if resp != []:
                return resp
        x = x+1
    return []

ans = 1
for x in  SearchSum(inp.getInput(), [], 2020, 2):
    ans = ans*x
print("part1: "+str(ans))

ans = 1
for x in  SearchSum(inp.getInput(), [], 2020, 3):
    ans = ans*x
print("part2: "+str(ans))