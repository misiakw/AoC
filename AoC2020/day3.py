from input import MapInput, Map
import sys

inp = MapInput(sys.argv[1])

def Path(chart: Map, moveX, moveY) -> int:
    x = 0
    y = 0
    trees = 0
    while y+moveY < chart.height:
        if x+moveX >= chart.width:
            chart.expandCopyRight()
        x = x+moveX
        y += moveY
        if chart.map[(x, y)] == '.':
            chart.map[(x, y)] = 'O'
        else:
            chart.map[(x, y)] = 'X'
            trees += 1
    return trees

path11 = Path(inp.map.copy(), 1, 1)
path31 = Path(inp.map.copy(), 3, 1)
path51 = Path(inp.map.copy(), 5, 1)
path71 = Path(inp.map.copy(), 7, 1)
path12 = Path(inp.map.copy(), 1, 2)

print("task1: ", path31)
print("task2: ", path11 * path31 * path51 * path71 * path12)