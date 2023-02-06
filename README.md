# Unity CustomTool
Simple Tools for Unity

### **List of Tools made**

+ **Snap Objects to position**  
    + Snap to position (x,y,z)
    + Can snap multiple objects in a single position
    + Undo implemented
    
+ **Rename selected object(s)**  
    + Can input name and number: `[name] [number]`
    + `[number]` increments if there are multple selected objects
      + [number] accepts symbols
      + **IF** both numbers and symbols are added, the number at the right most side will be incremented
        > Example:   
        > If [name] = Player and [number] = 00--01 => **Player 00--01**   
        > The next object will be named = **Player 00--02**
    + Undo implemented
    
+ **Sort Selected Objects by name in hierarchy**  
    + Sort by name
    + Does not break parent-children
    + Undo **\*NOT\* implemented**
