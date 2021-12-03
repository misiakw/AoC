from input import *
import sys

inp = RawInput(sys.argv[1])

sx=0
sy=0
rx=0
ry=0
ox = 0
oy = 0
chart = { (0, 0): 1 }
rChart = { (0, 0): 2 }

isSanta = 1
for direction in inp.raw:
    mods = [0, 0]
    if direction == '<':
        mods[0] = -1
    if direction ==  '>':
        mods[0] = 1
    if direction ==  '^':
        mods[1] = 1
    if direction ==  'v':
        mods[1] = -1

    x = 0
    y = 0
    ox = ox+mods[0]
    oy = oy+mods[1]
    if isSanta:
        sx = sx+mods[0]
        sy = sy+mods[1]
        x = sx
        y = sy
        isSanta = 0
    else:
        rx = rx+mods[0]
        ry = ry+mods[1]
        x = rx
        y = ry
        isSanta = 1

    presents = chart.get((ox, oy), 0)
    chart[(ox, oy)] = presents + 1
    rPresents = chart.get((x, y), 0)
    rChart[(x, y)] = rPresents + 1

print("part1: "+str(len(chart)))
print("part1: "+str(len(rChart)))