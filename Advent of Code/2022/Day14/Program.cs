using AoC.Common;
using System.Diagnostics.Contracts;

using var client = new AoC.Api.AoCClient();
var walls = await client.GetParsedAsync(2022, 14, p => p.Split(" -> ").Select(p=>p.Split(",").Select(Int32.Parse).ToArray()).Select(p=>new Point(p[0],p[1])).ToArray());
var allPoints = walls.SelectMany(p=>p);
 var minX = allPoints.Min(p=>p.X);
 var minY = allPoints.Min(p=>p.Y);
var maxX = allPoints.Max(p=>p.X);
var maxY = allPoints.Max(p=>p.Y);

//if(minY > 0) minY = 0;

var map = new int[maxX+1, maxY+1];

map[500, 0] = 1;
foreach(var wall in walls)
{
    foreach(var pair in wall.Window(2).Select(p=>p.ToList()))
    {
        for(int x = Math.Min(pair[0].X, pair[1].X); x <= Math.Max(pair[0].X, pair[1].X); x++)
        for(int y = Math.Min(pair[0].Y, pair[1].Y); y <= Math.Max(pair[0].Y, pair[1].Y); y++)
            map[x,y] = 2;        
    }
}

Point sandPos = new(500, 0);
var restPoints = 0;

while (true)
{
    if (sandPos.Y >= maxY) break;
    else if (map[sandPos.X, sandPos.Y + 1] != 2 && map[sandPos.X, sandPos.Y + 1] != 3)
        sandPos = sandPos with { Y = sandPos.Y + 1 };
    else if (map[sandPos.X - 1, sandPos.Y + 1] != 2 && map[sandPos.X - 1, sandPos.Y + 1] != 3)
        sandPos = sandPos with { Y = sandPos.Y + 1, X = sandPos.X - 1 };
    else if (map[sandPos.X + 1, sandPos.Y + 1] != 2 && map[sandPos.X + 1, sandPos.Y + 1] != 3)
        sandPos = sandPos with { Y = sandPos.Y + 1, X = sandPos.X + 1 };
    else
    {
        map[sandPos.X, sandPos.Y] = 3;
        sandPos = new(500, 0);
        restPoints++;
    }
}

SAK.DrawMap(map, window: (new(minX, 0), new (maxX + 1, maxY + 1)));
Console.WriteLine($"Resting sand count: " + restPoints);

map = new int[maxX + maxY, maxY + 3];
foreach (var wall in walls)
{
    foreach (var pair in wall.Window(2).Select(p => p.ToList()))
    {
        for (int x = Math.Min(pair[0].X, pair[1].X); x <= Math.Max(pair[0].X, pair[1].X); x++)
            for (int y = Math.Min(pair[0].Y, pair[1].Y); y <= Math.Max(pair[0].Y, pair[1].Y); y++)
                map[x, y] = 2;
    }
}
for (int x = 0; x < maxX+maxY; x++)
    map[x, maxY + 2] = 2;

restPoints = 0;
while (true)
{    
    if (map[sandPos.X, sandPos.Y + 1] != 2 && map[sandPos.X, sandPos.Y + 1] != 3)
        sandPos = sandPos with { Y = sandPos.Y + 1 };
    else if (map[sandPos.X - 1, sandPos.Y + 1] != 2 && map[sandPos.X - 1, sandPos.Y + 1] != 3)
        sandPos = sandPos with { Y = sandPos.Y + 1, X = sandPos.X - 1 };
    else if (map[sandPos.X + 1, sandPos.Y + 1] != 2 && map[sandPos.X + 1, sandPos.Y + 1] != 3)
        sandPos = sandPos with { Y = sandPos.Y + 1, X = sandPos.X + 1 };
    else
    {
        map[sandPos.X, sandPos.Y] = 3;
        if (sandPos == new Point (500, 0))
            break;
        sandPos = new(500, 0);
        restPoints++;
    }
}

SAK.DrawMap(map, window: (new(minX - maxY, 0), new(maxX + maxY, maxY + 3)));
Console.WriteLine($"Part 2: {restPoints + 1}");
/* FINAL MAP A:
                                            +                
                                                             
                                                             
                                                             
                                                             
                                                             
                                                             
                                          ++                 
                                         ++++                
                                        ++++++               
                                       ++++++++              
                                      ++++++++++             
                                     ++++++++++++            
                                    ++++######++++           
                                   ++++++    ++++++          
                                  +++###### ######++         
                                 +++++           ++++        
                                ++###### ###### ######       
                               ++++                          
                              ++++++                         
                             ++#####+                        
                            ++++   +++                       
                           +##### #####                      
                          +++                                
                         ##### ##### #####                   
                                                             
                      ##### ##### ##### #####                
                                                             
                                                      +      
                                                     #+#     
                                                    +#+#     
                                                   ++#+#     
                                                  +++#+#     
                                                 #####+######
                                                 #   +++    #
                                                +#  +++++   #
                                               ++# +++++++  #
                                              +++#+++++++++ #
                                             ++++#++++++++++#
                                            +++++############
                                           +++++++           
                                          +++++++++          
                                         +++++++++++     #   
                                        ++++++############   
                                       ++++++++              
                                      ++++++++++             
                                     ++++++++++++            
                                    +++++++++++++#           
                                   ###############           
                                  +                          
                                 +++                         
                                +#+#+                        
                               ++#+#++                       
                              +++#+#+++                      
                             #####+########                  
                             #   +++      #                  
                             #  +++++     #                  
                            +# +++++++    #                  
                           ++#+++++++++   #                  
                          +++##############                  
                         +++++                               
                        +++++++                              
                       ++######+                             
                      ++++    +++                            
                     +###### ######                          
                    +++                                      
                   ###### ###### ######                      
                                                             
                  +                                          
                 #++                                         
                 #+++                                        
                 #++++                                       
                +#+++++                                      
               #+#++++++                                     
               #+#+++++++                                    
               #+#++++++++                                   
               #+#+++++++++                                  
              +#+#+#++++++++                                 
             ++#+#+#+++++++++                                
            +++#####++++++++++                               
           +++++   ++++++++++++                              
          #++++++ ++++++++++++#+                             
          #####################++                            
                              ++++                           
                             ++++++                          
                            #+++++#+                         
                            #+++++#++                        
                            #+++++#+++                       
                         ####+++++#########                  
                         #  +++++++       #                  
                         # +++++++++      #                  
                         #+++++++++++     #                  
                         #++++++++++++    #                  
        ++               ##################                  
       ++++                                                  
      ++++++                                                 
     ++++++++                                                
    #+++++++++                                               
  # #++++++++++                                              
# # #+++#+++++++                                             
# # #+++#+++++++#                                            
# # #+++#+++#+++#                                            
# # #+++#+++#+++#                                            
# # #+++#+#+#+#+#                                            
# # #+++#+#+#+#+#                                            
# # #+#+#+#+#+#+#                                            
#################                                            
                                                             
                ++                                           
              #+++#                                          
              #+++#                                          
              #+++#+                                         
          #####+++###                                        
          #   +++++ #                                        
          #  +++++++#                                        
          # ++++++++#+                                       
          #+++++++++#++                                      
          #+++++++++#+++                                     
          #+++++++++#++++                                    
          #+++++++++#+++++                                   
          ###########++++++                                  
                    ++++++++                                 
             #     +++++++++#                                
             ################                                
                            ++                               
                           ++++                              
                          ######                             
                                +                            
                               +++                           
                       ###### ######                         
                             +                               
                            +++                              
                    ###### ###### ######                     
                          +                                  
                         +++                                 
                 ###### ###### ###### ######                 
                                                             
                                                             
                    #                                        
                    #                                        
                    #                                        
                    #                                        
                    #                                        
                    #                                        
                    # #                                      
                    # #+                                     
                    # #+#                                    
                    # #+#                                    
                    #####                                    
                                                             
                                                             
                                                             
                                                             
                                                             
                       #                                     
                       #+#                                   
                   #   #+#+                                  
                   #   #+#++                                 
                   #   #+#+#+                                
                   #   #+#+#+#                               
                   # # #+#+#+#                               
                   ###########  

 
 FINAL MAP B

                                                                                                                                                                   o                                                                                                                                                                   
                                                                                                                                                                  ooo                                                                                                                                                                  
                                                                                                                                                                 ooooo                                                                                                                                                                 
                                                                                                                                                                ooooooo                                                                                                                                                                
                                                                                                                                                               ooooooooo                                                                                                                                                               
                                                                                                                                                              ooooooooooo                                                                                                                                                              
                                                                                                                                                             ooooooooooooo                                                                                                                                                             
                                                                                                                                                            ooooooooooooooo                                                                                                                                                            
                                                                                                                                                           ooooooooooooooooo                                                                                                                                                           
                                                                                                                                                          ooooooooooooooooooo                                                                                                                                                          
                                                                                                                                                         ooooooooooooooooooooo                                                                                                                                                         
                                                                                                                                                        ooooooooooooooooooooooo                                                                                                                                                        
                                                                                                                                                       ooooooooooooooooooooooooo                                                                                                                                                       
                                                                                                                                                      oooooooooo######ooooooooooo                                                                                                                                                      
                                                                                                                                                     oooooooooooo    ooooooooooooo                                                                                                                                                     
                                                                                                                                                    ooooooooo###### ######ooooooooo                                                                                                                                                    
                                                                                                                                                   ooooooooooo           ooooooooooo                                                                                                                                                   
                                                                                                                                                  oooooooo###### ###### ######ooooooo                                                                                                                                                  
                                                                                                                                                 oooooooooo                  ooooooooo                                                                                                                                                 
                                                                                                                                                oooooooooooo                ooooooooooo                                                                                                                                                
                                                                                                                                               oooooooo#####o              ooooooooooooo                                                                                                                                               
                                                                                                                                              oooooooooo   ooo            ooooooooooooooo                                                                                                                                              
                                                                                                                                             ooooooo##### #####          ooooooooooooooooo                                                                                                                                             
                                                                                                                                            ooooooooo                   ooooooooooooooooooo                                                                                                                                            
                                                                                                                                           oooooo##### ##### #####     ooooooooooooooooooooo                                                                                                                                           
                                                                                                                                          oooooooo                    ooooooooooooooooooooooo                                                                                                                                          
                                                                                                                                         ooooo##### ##### ##### #####ooooooooooooooooooooooooo                                                                                                                                         
                                                                                                                                        ooooooo                     ooooooooooooooooooooooooooo                                                                                                                                        
                                                                                                                                       ooooooooo                   ooooooooooooooooooooooooooooo                                                                                                                                       
                                                                                                                                      ooooooooooo                 ooooooooooo#o#ooooooooooooooooo                                                                                                                                      
                                                                                                                                     ooooooooooooo               oooooooooooo#o#oooooooooooooooooo                                                                                                                                     
                                                                                                                                    ooooooooooooooo             ooooooooooooo#o#ooooooooooooooooooo                                                                                                                                    
                                                                                                                                   ooooooooooooooooo           oooooooooooooo#o#oooooooooooooooooooo                                                                                                                                   
                                                                                                                                  ooooooooooooooooooo         ooooooooooo#####o######oooooooooooooooo                                                                                                                                  
                                                                                                                                 ooooooooooooooooooooo       oooooooooooo#   ooo    #ooooooooooooooooo                                                                                                                                 
                                                                                                                                ooooooooooooooooooooooo     ooooooooooooo#  ooooo   #oooooooooooooooooo                                                                                                                                
                                                                                                                               ooooooooooooooooooooooooo   oooooooooooooo# ooooooo  #ooooooooooooooooooo                                                                                                                               
                                                                                                                              ooooooooooooooooooooooooooo ooooooooooooooo#ooooooooo #oooooooooooooooooooo                                                                                                                              
                                                                                                                             oooooooooooooooooooooooooooooooooooooooooooo#oooooooooo#ooooooooooooooooooooo                                                                                                                             
                                                                                                                            ooooooooooooooooooooooooooooooooooooooooooooo############oooooooooooooooooooooo                                                                                                                            
                                                                                                                           ooooooooooooooooooooooooooooooooooooooooooooooo          oooooooooooooooooooooooo                                                                                                                           
                                                                                                                          ooooooooooooooooooooooooooooooooooooooooooooooooo        oooooooooooooooooooooooooo                                                                                                                          
                                                                                                                         ooooooooooooooooooooooooooooooooooooooooooooooooooo     #oooooooooooooooooooooooooooo                                                                                                                         
                                                                                                                        oooooooooooooooooooooooooooooooooooooooooooooo############ooooooooooooooooooooooooooooo                                                                                                                        
                                                                                                                       oooooooooooooooooooooooooooooooooooooooooooooooo          ooooooooooooooooooooooooooooooo                                                                                                                       
                                                                                                                      oooooooooooooooooooooooooooooooooooooooooooooooooo        ooooooooooooooooooooooooooooooooo                                                                                                                      
                                                                                                                     oooooooooooooooooooooooooooooooooooooooooooooooooooo      ooooooooooooooooooooooooooooooooooo                                                                                                                     
                                                                                                                    ooooooooooooooooooooooooooooooooooooooooooooooooooooo#    ooooooooooooooooooooooooooooooooooooo                                                                                                                    
                                                                                                                   oooooooooooooooooooooooooooooooooooooooo###############   ooooooooooooooooooooooooooooooooooooooo                                                                                                                   
                                                                                                                  oooooooooooooooooooooooooooooooooooooooooo                ooooooooooooooooooooooooooooooooooooooooo                                                                                                                  
                                                                                                                 oooooooooooooooooooooooooooooooooooooooooooo              ooooooooooooooooooooooooooooooooooooooooooo                                                                                                                 
                                                                                                                ooooooooooooooooooooooooooooooooooooooooo#o#oo            ooooooooooooooooooooooooooooooooooooooooooooo                                                                                                                
                                                                                                               oooooooooooooooooooooooooooooooooooooooooo#o#ooo          ooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                               
                                                                                                              ooooooooooooooooooooooooooooooooooooooooooo#o#oooo        ooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                              
                                                                                                             oooooooooooooooooooooooooooooooooooooooo#####o########    ooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                             
                                                                                                            ooooooooooooooooooooooooooooooooooooooooo#   ooo      #   ooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                            
                                                                                                           oooooooooooooooooooooooooooooooooooooooooo#  ooooo     #  ooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                           
                                                                                                          ooooooooooooooooooooooooooooooooooooooooooo# ooooooo    # ooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                          
                                                                                                         oooooooooooooooooooooooooooooooooooooooooooo#ooooooooo   #ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                         
                                                                                                        ooooooooooooooooooooooooooooooooooooooooooooo##############oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                        
                                                                                                       ooooooooooooooooooooooooooooooooooooooooooooooo            oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                       
                                                                                                      ooooooooooooooooooooooooooooooooooooooooooooooooo          oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                      
                                                                                                     oooooooooooooooooooooooooooooooooooooooooooo######o        oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                     
                                                                                                    oooooooooooooooooooooooooooooooooooooooooooooo    ooo      oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                    
                                                                                                   ooooooooooooooooooooooooooooooooooooooooooo###### ######   oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                   
                                                                                                  ooooooooooooooooooooooooooooooooooooooooooooo              oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                  
                                                                                                 oooooooooooooooooooooooooooooooooooooooooo###### ###### ######ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                 
                                                                                                oooooooooooooooooooooooooooooooooooooooooooo                  ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                                
                                                                                               oooooooooooooooooooooooooooooooooooooooooooooo                ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                               
                                                                                              ooooooooooooooooooooooooooooooooooooooooooo#oooo              ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                              
                                                                                             oooooooooooooooooooooooooooooooooooooooooooo#ooooo            ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                             
                                                                                            ooooooooooooooooooooooooooooooooooooooooooooo#oooooo          ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                            
                                                                                           oooooooooooooooooooooooooooooooooooooooooooooo#ooooooo        ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                           
                                                                                          ooooooooooooooooooooooooooooooooooooooooooooo#o#oooooooo      ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                          
                                                                                         oooooooooooooooooooooooooooooooooooooooooooooo#o#ooooooooo    ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                         
                                                                                        ooooooooooooooooooooooooooooooooooooooooooooooo#o#oooooooooo  ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                        
                                                                                       oooooooooooooooooooooooooooooooooooooooooooooooo#o#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                       
                                                                                      ooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                      
                                                                                     oooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                     
                                                                                    ooooooooooooooooooooooooooooooooooooooooooooooooooo#####ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                    
                                                                                   ooooooooooooooooooooooooooooooooooooooooooooooooooooo   ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                   
                                                                                  oooooooooooooooooooooooooooooooooooooooooooooooo#oooooo oooooooooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                  
                                                                                 ooooooooooooooooooooooooooooooooooooooooooooooooo#####################ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                 
                                                                                ooooooooooooooooooooooooooooooooooooooooooooooooooo                   ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                                
                                                                               ooooooooooooooooooooooooooooooooooooooooooooooooooooo                 ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                               
                                                                              ooooooooooooooooooooooooooooooooooooooooooooooooooooooo               #ooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                              
                                                                             ooooooooooooooooooooooooooooooooooooooooooooooooooooooooo              #ooooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                             
                                                                            ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo             #ooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                            
                                                                           ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo         ####ooooo#########ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                           
                                                                          ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo        #  ooooooo       #oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                          
                                                                         ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo       # ooooooooo      #ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                         
                                                                        ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo      #ooooooooooo     #oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                        
                                                                       ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo     #oooooooooooo    #ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                       
                                                                      ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo    ##################oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                      
                                                                     ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                    oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                     
                                                                    ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                  oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                    
                                                                   ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                   
                                                                  oooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#oooooooooooooooooooo              oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                  
                                                                 ooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#ooooooooooooooooooooo            oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                 
                                                                oooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#ooo#oooooooooooooooooo          oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                                
                                                               ooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#ooo#ooooooo#ooooooooooo        oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                               
                                                              oooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#ooo#ooo#ooo#oooooooooooo      oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                              
                                                             ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#ooo#ooo#ooo#ooooooooooooo    oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                             
                                                            oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#ooo#o#o#o#o#oooooooooooooo  oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                            
                                                           ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#ooo#o#o#o#o#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                           
                                                          oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#o#o#o#o#o#o#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                          
                                                         ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#################ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                         
                                                        ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo               ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                        
                                                       ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo             ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                       
                                                      ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo           #ooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                      
                                                     ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo          #ooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                     
                                                    ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo         #ooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                    
                                                   ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo    #####ooo###ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                   
                                                  ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo   #   ooooo #oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                  
                                                 ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo  #  ooooooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                 
                                                ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo # oooooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                                
                                               ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooooooooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                               
                                              oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooooooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                              
                                             ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooooooooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                             
                                            oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooooooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                            
                                           ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo###########ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                           
                                          ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo         ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                          
                                         ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo #     ooooooooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                         
                                        ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo################oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                        
                                       ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo              oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                       
                                      ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo            oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                      
                                     ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo          ######oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                     
                                    ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo              oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                    
                                   ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo            oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                   
                                  ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo    ######o######ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                  
                                 ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo        ooo    ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                 
                                ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo      ooooo  ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                                
                               ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo######o######o######oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                               
                              ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo    ooo    ooo    oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                              
                             ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo  ooooo  ooooo  oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                             
                            ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo######o######o######o######ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                            
                           ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo    ooo    ooo    ooo    ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                           
                          ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo  ooooo  ooooo  ooooo  ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                          
                         ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                         
                        oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                        
                       ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                       
                      oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                      
                     ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                     
                    oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                    
                   ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                   
                  oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                  
                 ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                 
                oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo                
               ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#####ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo               
              ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo   ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo              
             ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo             
            ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo            
           ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo           
          ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo          
         oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo         
        ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo        
       oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooo#o#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo       
      ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooo#o#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo      
     oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooo#o#o#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo     
    ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#ooo#o#o#o#ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo    
   oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo#o#o#o#o#o#oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo   
  ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo###########ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo  
 ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo         ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo 
#######################################################################################################################################################################################################################################################################################################################################
 */