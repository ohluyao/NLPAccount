##Test Mard Down##

###1. Configure NLPIR###
- Copy the Data directory and NLPIR.dll to exe.


###2. Pattern ###
Pattern is used to match a sentence to account. There are two types of match method, match by segment type and match by segment value.

*Example:* "(/n?) /p? (/x) /v (/daxue) (/ct) /v (/n) 花费 (/m) (/q)"

Pattern segment is split by *space*, 

- Match by Segment Type
Segment match by type is starts with *"/"*, eg: /n. The word followed by *"/"* is the word type. Word type can be found in the document *ICTPOS3.0.doc*.

- Match by Segment Value
Segment match by value is a normal string, eg: 花费.

###3. Action ###
Action is used to fill the matched segment as Account properties sunch as user,position,cost etc.

*Example:* "{type:支出 user:/1 datetime:/2 position:/3+/4 cost:/6+/7}"

Action is surrounded by blaknet and each action is split by *space*. For each action, it is a dictionary, key is account property name and value is account property's value. 

- Account property
eg: type, user, datetime, position, cost.

- Account property's Value
eg: position:/3+/4
-- Value can be *number* or *normal string*.
-- The *number* indicate the index of the mateced segment by pattern and is prefixed a *"/"*. 
-- A *normal string* indicate the value that can be used directlly.
-- A value can be consist of multiple matched segment or value user provided, and these segment are concate with *"+"*. eg: /3+/4
  
