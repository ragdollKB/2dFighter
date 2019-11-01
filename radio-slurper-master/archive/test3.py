import re 

clip="good for you why when night of a movie of a new road only get at the moon mind and we went out and you'll be found on the inland the a to say of what that you choose what's the advice that cash that it would reiterate the code word is mouth that could work his nose animal you and see that they were visibly angry that rates apply if the fifty and heidstra india india and nailing and all the way to bed only say it but on him then isn't it a long moved uneasily wound in the louvre it on the n. gone beyond what has is up to one of the move an and we just found and the and i didn't have the hudson ahead and we'll stay in power and none is playing our hometown of my name's on it and it's not o. and and you get uh a lot what is it the hindu have a p. and pound get an inept and i didn't i have baggage that they still would agree and who are to with the code word is nose and on you and the things that were visible to the one you would agree that it's applied to throw it"

r = r"code\sword\sis.*?(?=\w)(\w+)"
m = re.findall(r, clip)
print(m)