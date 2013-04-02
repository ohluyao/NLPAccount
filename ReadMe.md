##NLP Account##

###1. Configure NLPIR###
- Copy the Data directory and NLPIR.dll to exe.


###2. Pattern ###
Pattern is used to match a sentence to account. There are two types of match method, match by segment type and match by segment value.

**Example:** 		
    
	"(/n?) /p? (/x|/m) /v (/daxue) (/ct) /v (/n) 花费 (/m) (/q)"

 Pattern segment is split by **space**, 

- Each pattern segment can consist of multiple segments which separated by `|`.If one of the segment can be matched, the whole pattern segment are matched.

- Match by Segment Type   
Segment match by type is starts with *"/"*, **eg:** `/n`.  
The word followed by `/` is the word type. Word type can be found in the document **"ICTPOS3.0.doc"**.

- Match by Segment Value   
Segment match by value is a normal string, **eg:** `花费`.

- Match and save  
Segment surrounded by parenthesis `()` should be matched and saved. Saved segments will be used for action later. 

- Question Mark `?` in Pattern  
Question Mark in Pattern means optional. **eg:**`(/n?)`, This will match if the sentence have a *none* type word or not.

###3. Action ###
Action is used to fill the matched segment as Account properties such as `user,position,cost` etc.

**Example:** 

    
	"{type:支出 user:/1 datetime:/2 position:/3+/4 cost:/6+/7}"

Action is surrounded by blanket `{}` and each action is split by **space**. For each action, it is a dictionary, key is account property name and value is account property's value. 

- Account property
**eg:** `type, user, datetime, position, cost`.

- Account property's Value **eg:** `position:/3+/4`
 1. This can be a list of values separated by `|` **eg:** `user:/1|我`       
     - The values are matched according to its order in the value list.
     
     - The match will stop once the matched segment indicated by the **number** is not empty or the value is **normal string**   
  
 
 2. Value can be **number** or **normal string**.  
     - A **number** indicate the index of the matched segment by pattern and is prefixed a `/`. 
  
     - A **normal string** indicate the value that can be used directly.
 3. A value can be consist of multiple matched segment or value user provided, and these segment are concatenate with `+`. **eg:** `/3+/4`
