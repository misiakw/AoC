from input import RawInput
import sys

inp = RawInput(sys.argv[1])

class QueueItem:
    def __init__(self, pre, val, post):
        self.pre = None
        self.val = val
        self.post = None
    def __repr__(self): return str(self.val)
class Queue:
    def __init__(self):
        self.size = 0
        self.bottom = None
        self.top = self.bottom
    def push(self, val):
        self.size += 1
        item = QueueItem(self.bottom, val, None)
        if self.bottom is not None:
            self.bottom.post = item
        if self.top is None:
            self.top = item
        self.bottom = item
    def pop(self):
        if self.top is None:
            return None
        self.size -= 1
        result = self.top
        self.top = result.post
        if self.top is not None:
            self.top.pre = None
        return result.val
    def getScore(self):
        result = 0
        i = self.size
        x = self.top
        while x is not None:
            result += x.val*i
            i-=1
            x = x.post
        return result
    def clone(self, size):
        result = Queue()
        x = self.top
        while x is not None and size > 0:
            result.push(x.val)
            x = x.post
            size -= 1
        return result
    def __repr__(self):
        result = ""
        x = self.top
        while x != None:
            result+=str(x)
            if x.post is not None:
                result+=", "
            x = x.post
        return result
class Game:
    def __init__(self, decks):
        self.deck1 = decks[0]
        self.deck2 = decks[1]
    def CanNext(self)->bool: return self.deck1.size > 0 and self.deck2.size > 0
    def Round(self):
        p1 = self.deck1.pop()
        p2 = self.deck2.pop()
        self.validateRound(p1, p2)
    def validateRound(self, p1, p2):
        if p1 > p2:
            self.deck1.push(p1)
            self.deck1.push(p2)
        else:
            self.deck2.push(p2)
            self.deck2.push(p1)
class RecGame(Game):
    gameNum = 1
    def __init__(self, decks):
        super(RecGame, self).__init__(decks)
        self.states = []
        self.num = self.gameNum
        RecGame.gameNum +=1
        self.wasDeathloop = False
        self.round = 1
    def CanNext(self)->bool: return not self.wasDeathloop and self.deck1.size > 0 and self.deck2.size > 0
    def Round(self):
        self.isDeathLoop()
        if self.wasDeathloop:
            return
        p1 = self.deck1.pop()
        p2 = self.deck2.pop()
        won = 1
        if p1 <= self.deck1.size and p2 <= self.deck2.size:#) or (p2 == self.deck2.size and p1 <= self.deck1.size):#do sub game
            deck1 = self.deck1.clone(p1)
            deck2 = self.deck2.clone(p2)
            subGame = RecGame((deck1, deck2))
            while subGame.CanNext(): subGame.Round()
            if subGame.deck2.size == 0 or subGame.wasDeathloop:
                self.deck1.push(p1)
                self.deck1.push(p2)
            else:
                won = 2
                self.deck2.push(p2)
                self.deck2.push(p1)

        else: #do normal game
            if p1 < p2:
                won = 2
            self.validateRound(p1, p2)
        self.round += 1
    def isDeathLoop(self):
        state = str(self.deck1)+"|"+str(self.deck2)
        if state in self.states:
            #print("player 1 wins by deathloop ", self.num)
            self.wasDeathloop = True
        else:
            self.states.append(state)
def getStartingHand()->(Queue, Queue):
    deck1 = Queue()
    deck2 = Queue()
    deck = deck1
    for player in inp.getRaw().split("\n\n"):
        lines = player.split("\n")
        for i in range(1, len(lines)):
            deck.push(int(lines[i]))
        deck = deck2
    return (deck1, deck2)

game1 = Game(getStartingHand())
while game1.CanNext(): game1.Round()
deck = game1.deck2 if game1.deck1.size == 0 else game1.deck1
print("Part1", deck.getScore())

game2 = RecGame(getStartingHand())
while game2.CanNext(): game2.Round()
deck = game2.deck2 if game2.deck1.size == 0 else game2.deck1
print("Part2", deck.getScore())