# Case: FiveWordFiveLetters
Opgave: https://www.youtube.com/watch?v=_-AfhLQfb6w&t=860s
I skal lave et program der kan finde alle de ordkombinationer, hvor følgende er opfyldt:
- Der skal være 5 ord
- Alle ord skal have 5 bogstaver
- Hvert bogstav må kun optræde 1 gang

Da der i løbet af casen vil bliver undervist i forskellige principper for objektorienteret programmering og programmerings metodik gennemføres casen i en række delopgaver:

## Step 1: Indlæs data
De ord der skal bruges kan indlæses fra en fil.
Med inspiration fra “Make it work” skal i lave et proof of concept for indlæsning af data ved at følge følgende fremgangsmåde:
1. Lav en lille fil med perfekt data, et ord pr. linje
2. Indlæs filen
3. Skriv en test der validere at den rigtige data er indlæst
4. Udvid filen med uperfekt data
5. Refactor indlæsning
6. Kontrollere/refactor testen (Kan være der skal udvides med flere tests)
   ![billede](https://github.com/user-attachments/assets/67a41515-9cb9-43d0-9050-9783962b8d70)



## Step 2: Løs case med beta data
Færdiggør programmet så det virker med words_beta.txt

## Step 3: Make it right
Opfyld coding conventions og brug Exception handling
https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions


## Step 4: Make it Right
Lav opgaven collections
Refactor programmet med den nye viden I har fået

## Step 5: Make it fast
Refactor programmet til at bruge bit sammenligning frem for tekst

## Step 6: Make it fast
Refactor programmet til at bruge flere threads

## Step 7: Make it fast
Lav programmet så hurtig som muligt


## WPF eks.
- Binding i xaml fil
![billede](https://github.com/user-attachments/assets/08b62424-987e-457b-a3cd-b9bc74c8f88f)

- Implementer INotifyPropertyChanged
![billede](https://github.com/user-attachments/assets/cd34bddb-4566-4c07-8c44-374b2e5964ae)

- Event handler
![billede](https://github.com/user-attachments/assets/28e9d077-7a87-42fa-94b0-69db72fca576)

- Event i Class library thread (Husk at tilføj jeres ClassLibrary til jeres WPF (og console))
![billede](https://github.com/user-attachments/assets/2790863b-0f8e-4cbd-879d-e174dc530fc5)


## Step 8: GUI
Split logikken ud i et class library som både Konsol og Windows GUI kan bruge
Lav en GUI hvor det som minimum er muligt at vælge hvilken fil der skal hentes ord fra, vises en progressbar, info om antal muligheder fundet, mulighed for at gemme resultatet til fil.

## Ekstra:
Præsentere resultatet løbende i en list
Gør det muligt at ændre på kravene (antal bogstaver og antal ord) for at danne kombinationer


## Fremlæggelse
I skal lave en fremlæggelse af jeres løsning.

Inspiration til disponering af fremlæggelse:
- Hvad var opgaven I har løst (Ja vi ved det alle sammen, men det er alligevel en god starter)
- Demonstration af jeres løsning
- Konsol programmet
- Hastighed, hvor hurtig er løsningen blevet.
- GUI løsningen.
- Algoritme (Hvilke algoritmer (tricks) har I brugt for at gøre løsningen hurtig
- Kode demonstration evt. debug kritisk kode, med forklaring af hvad der sker hvis debug ikke er mulig, brug evt. debug udgave af løsningen til at udskrive hvad der sker.

## Evaluering: 
- Hvad har I lært…
- Hvilke udfordringer har I haft undervejs…

