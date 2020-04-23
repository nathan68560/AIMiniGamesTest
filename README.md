AI Mini Games Test is a Unity 3D project which consist of two different mini game. The first, D_AIG (Draw, AI Guess), is a game where you have 3 different symbols (cross, circle and triangle) that you can draw on a 32x32 pixels board. The AI will then try to guess which it is. The second one, TAICTOE, is as you guessed a classic tic tac toe game, you play the cross and the AI play the circle.


                                                        Technical details
D_AIG :

-The neural network used for D_AIG is composed of 320 inputs, those are 5 pre-made motifs for each 4x4 pixels chunks, 3 hidden layers of respectively 16, 32, then again 16 neurons, and finally 3 outputs, one for each symbols.

-The neural network use the classic feed forward method with weights for each connection between two neurons and each neurons also have it's own bias.

-The activation method used is the hyperbolic tangent (tanh) function.

-The learning process used is backpropagation, the cost or error function used for the backpropagation is mean((Output-Expected)²).

-The built-in learning set consist of 12 drawing for each of the three symbols. There's also a bigger learning set included called DAIG.dat, for the network to use it you have to put the file in 'C:\Users\USERNAME\AppData\LocalLow\Nathan_\AIMiniGamesTest'.


TAICTOE :

-At first i also tried to do it with a feed forward and backpropagation network, but the result i got were worst than expected even after a lot of training, so i opted for a minimax algorithm which seemed way more appropriate for this game.

-The minimax algorithm i used is greatly inspired from the one the YouTube chanel The Coding Train used for it's own Tic Tac Toe AI.

-Since i used the minimax algorythm alone, there's no way for TAICTOE to learn and he his from the begining 'trained' to win the game. I let one unique move that the player can do to win against the AI, so it still can be defeated if it turned evil ;).



                                                       Ressources used
                                                       
-The book 'Quand la machine apprend' from Yann Le Cun, in depth explaination for all different types of NN etc...

-The YouTube chanel of Jabrils, entertainment and idea for a AI game that i later abandonned

-The YouTube chanel of 3blue1brown, for all the mathematical explaination related to backpropagation

-The YouTube chanel of The Coding Train, for the functionnement of the minimax fucntion

-The YouTube chanel of Finn Eggers, for his video on how to code the backpropagation method
