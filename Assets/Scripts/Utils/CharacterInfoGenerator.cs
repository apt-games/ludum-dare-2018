using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class CharacterInfo
{
    public string name { get; set; }
    public Sprite avatar { get; set; }
    public CharacterColors colors { get; set; }
}

[Serializable]
public class CharacterColors
{
    public Color Head = Color.white;
    public Color Body = Color.white;
    public Color Legs = Color.white;
}


static class CharacterInfoGenerator {

    public static CharacterInfo getCharacterInfo()
    {
        if (Random.Range(0, 2) == 0)
        {
            var details = getFemale();
            return new CharacterInfo()
            {
                name = getFemaleName(),
                avatar = details.Avatar,
                colors = details.Colors,
            };
        }
        else
        {
            var details = getMale();
            return new CharacterInfo()
            {
                name = getMaleName(),
                avatar = details.Avatar,
                colors = details.Colors,
            };
        }
    }

    static string getFemaleName()
    {
        int index = Random.Range(0, femaleNames.Length);

        return femaleNames[index];
    }

    static string getMaleName()
    {
        int index = Random.Range(0, maleNames.Length);

        return maleNames[index];
    }

    static CharacterDetails getFemale()
    {
        var file = FemaleAvatarFilenames[Random.Range(0, MaleAvatarFilenames.Length)];
        return Resources.Load<CharacterDetails>(file);
    }

    static CharacterDetails getMale()
    {
        var file = MaleAvatarFilenames[Random.Range(0, MaleAvatarFilenames.Length)];
        return Resources.Load<CharacterDetails>(file);
    }


    static string[] femaleNames = new [] {
		"Mary",
		"Patricia",
		"Linda",
		"Barbara",
		"Elizabeth",
		"Jennifer",
		"Maria",
		"Susan",
		"Margaret",
		"Dorothy",
		"Lisa",
		"Nancy",
		"Karen",
		"Betty",
		"Helen",
		"Sandra",
		"Donna",
		"Carol",
		"Ruth",
		"Sharon",
		"Michelle",
		"Laura",
		"Sarah",
		"Kimberly",
		"Deborah",
		"Jessica",
		"Shirley",
		"Cynthia",
		"Angela",
		"Melissa",
		"Brenda",
		"Amy",
		"Anna",
		"Rebecca",
		"Virginia",
		"Kathleen",
		"Pamela",
		"Martha",
		"Debra",
		"Amanda",
		"Stephanie",
		"Carolyn",
		"Christine",
		"Marie",
		"Janet",
		"Catherine",
		"Frances",
		"Ann",
		"Joyce",
		"Diane",
		"Alice",
		"Julie",
		"Heather",
		"Teresa",
		"Doris",
		"Gloria",
		"Evelyn",
		"Jean",
		"Cheryl",
		"Mildred",
		"Katherine",
		"Joan",
		"Ashley",
		"Judith",
		"Rose",
		"Janice",
		"Kelly",
		"Nicole",
		"Judy",
		"Christina",
		"Kathy",
		"Theresa",
		"Beverly",
		"Denise",
		"Tammy",
		"Irene",
		"Jane",
		"Lori",
		"Rachel",
		"Marilyn",
		"Andrea",
		"Kathryn",
		"Louise",
		"Sara",
		"Anne",
		"Jacqueline",
		"Wanda",
		"Bonnie",
		"Julia",
		"Ruby",
		"Lois",
		"Tina",
		"Phyllis",
		"Norma",
		"Paula",
		"Diana",
		"Annie",
		"Lillian",
		"Emily",
		"Robin",
		"Peggy",
		"Crystal",
		"Gladys",
		"Rita",
		"Dawn",
		"Connie",
		"Florence",
		"Tracy",
		"Edna",
		"Tiffany",
		"Carmen",
		"Rosa",
		"Cindy",
		"Grace",
		"Wendy",
		"Victoria",
		"Edith",
		"Kim",
		"Sherry",
		"Sylvia",
		"Josephine",
		"Thelma",
		"Shannon",
		"Sheila",
		"Ethel",
		"Ellen",
		"Elaine",
		"Marjorie",
		"Carrie",
		"Charlotte",
		"Monica",
		"Esther",
		"Pauline",
		"Emma",
		"Juanita",
		"Anita",
		"Rhonda",
		"Hazel",
		"Amber",
		"Eva",
		"Debbie",
		"April",
		"Leslie",
		"Clara",
		"Lucille",
		"Jamie",
		"Joanne",
		"Eleanor",
		"Valerie",
		"Danielle",
		"Megan",
		"Alicia",
		"Suzanne",
		"Michele",
		"Gail",
		"Bertha",
		"Darlene",
		"Veronica",
		"Jill",
		"Erin",
		"Geraldine",
		"Lauren",
		"Cathy",
		"Joann",
		"Lorraine",
		"Lynn",
		"Sally",
		"Regina",
		"Erica",
		"Beatrice",
		"Dolores",
		"Bernice",
		"Audrey",
		"Yvonne",
		"Annette",
		"June",
		"Samantha",
		"Marion",
		"Dana",
		"Stacy",
		"Ana",
		"Renee",
		"Ida",
		"Vivian",
		"Roberta",
		"Holly",
		"Brittany",
		"Melanie",
		"Loretta",
		"Yolanda",
		"Jeanette",
		"Laurie",
		"Katie",
		"Kristen",
		"Vanessa",
		"Alma",
		"Sue",
		"Elsie",
		"Beth",
		"Jeanne",
		"Vicki",
		"Carla",
		"Tara",
		"Rosemary",
		"Eileen",
		"Terri",
		"Gertrude",
		"Lucy",
		"Tonya",
		"Ella",
		"Stacey",
		"Wilma",
		"Gina",
		"Kristin",
		"Jessie",
		"Natalie",
		"Agnes",
		"Vera",
		"Willie",
		"Charlene",
		"Bessie",
		"Delores",
		"Melinda",
		"Pearl",
		"Arlene",
		"Maureen",
		"Colleen",
		"Allison",
		"Tamara",
		"Joy",
		"Georgia",
		"Constance",
		"Lillie",
		"Claudia",
		"Jackie",
		"Marcia",
		"Tanya",
		"Nellie",
		"Minnie",
		"Marlene",
		"Heidi",
		"Glenda",
		"Lydia",
		"Viola",
		"Courtney",
		"Marian",
		"Stella",
		"Caroline",
		"Dora",
		"Jo",
		"Vickie",
		"Mattie",
		"Terry",
		"Maxine",
		"Irma",
		"Mabel",
		"Marsha",
		"Myrtle",
		"Lena",
		"Christy",
		"Deanna",
		"Patsy",
		"Hilda",
		"Gwendolyn",
		"Jennie",
		"Nora",
		"Margie",
		"Nina",
		"Cassandra",
		"Leah",
		"Penny",
		"Kay",
		"Priscilla",
		"Naomi",
		"Carole",
		"Brandy",
		"Olga",
		"Billie",
		"Dianne",
		"Tracey",
		"Leona",
		"Jenny",
		"Felicia",
		"Sonia",
		"Miriam",
		"Velma",
		"Becky",
		"Bobbie",
		"Violet",
		"Kristina",
		"Toni",
		"Misty",
		"Mae",
		"Shelly",
		"Daisy",
		"Ramona",
		"Sherri",
		"Erika",
		"Katrina",
		"Claire",
		"Lindsey",
		"Lindsay",
		"Geneva",
		"Guadalupe",
		"Belinda",
		"Margarita",
		"Sheryl",
		"Cora",
		"Faye",
		"Ada",
		"Natasha",
		"Sabrina",
		"Isabel",
		"Marguerite",
		"Hattie",
		"Harriet",
		"Molly",
		"Cecilia",
		"Kristi",
		"Brandi",
		"Blanche",
		"Sandy",
		"Rosie",
		"Joanna",
		"Iris",
		"Eunice",
		"Angie",
		"Inez",
		"Lynda",
		"Madeline",
		"Amelia",
		"Alberta",
		"Genevieve",
		"Monique",
		"Jodi",
		"Janie",
		"Maggie",
		"Kayla",
		"Sonya",
		"Jan",
		"Lee",
		"Kristine",
		"Candace",
		"Fannie",
		"Maryann",
		"Opal",
		"Alison",
		"Yvette",
		"Melody",
		"Luz",
		"Susie",
		"Olivia",
		"Flora",
		"Shelley",
		"Kristy",
		"Mamie",
		"Lula",
		"Lola",
		"Verna",
		"Beulah",
		"Antoinette",
		"Candice",
		"Juana",
		"Jeannette",
		"Pam",
		"Kelli",
		"Hannah",
		"Whitney",
		"Bridget",
		"Karla",
		"Celia",
		"Latoya",
		"Patty",
		"Shelia",
		"Gayle",
		"Della",
		"Vicky",
		"Lynne",
		"Sheri",
		"Marianne",
		"Kara",
		"Jacquelyn",
		"Erma",
		"Blanca",
		"Myra",
		"Leticia",
		"Pat",
		"Krista",
		"Roxanne",
		"Angelica",
		"Johnnie",
		"Robyn",
		"Francis",
		"Adrienne",
		"Rosalie",
		"Alexandra",
		"Brooke",
		"Bethany",
		"Sadie",
		"Bernadette",
		"Traci",
		"Jody",
		"Kendra",
		"Jasmine",
		"Nichole",
		"Rachael",
		"Chelsea",
		"Mable",
		"Ernestine",
		"Muriel",
		"Marcella",
		"Elena",
		"Krystal",
		"Angelina",
		"Nadine",
		"Kari",
		"Estelle",
		"Dianna",
		"Paulette",
		"Lora",
		"Mona",
		"Doreen",
		"Rosemarie",
		"Angel",
		"Desiree",
		"Antonia",
		"Hope",
		"Ginger",
		"Janis",
		"Betsy",
		"Christie",
		"Freda",
		"Mercedes",
		"Meredith",
		"Lynette",
		"Teri",
		"Cristina",
		"Eula",
		"Leigh",
		"Meghan",
		"Sophia",
		"Eloise",
		"Rochelle",
		"Gretchen",
		"Cecelia",
		"Raquel",
		"Henrietta",
		"Alyssa",
		"Jana",
		"Kelley",
		"Gwen",
		"Kerry",
		"Jenna",
		"Tricia",
		"Laverne",
		"Olive",
		"Alexis",
		"Tasha",
		"Silvia",
		"Elvira",
		"Casey",
		"Delia",
		"Sophie",
		"Kate",
		"Patti",
		"Lorena",
		"Kellie",
		"Sonja",
		"Lila",
		"Lana",
		"Darla",
		"May",
		"Mindy",
		"Essie",
		"Mandy",
		"Lorene",
		"Elsa",
		"Josefina",
		"Jeannie",
		"Miranda",
		"Dixie",
		"Lucia",
		"Marta",
		"Faith",
		"Lela",
		"Johanna",
		"Shari",
		"Camille",
		"Tami",
		"Shawna",
		"Elisa",
		"Ebony",
		"Melba",
		"Ora",
		"Nettie",
		"Tabitha",
		"Ollie",
		"Jaime",
		"Winifred",
		"Kristie"
	};

	static string[] maleNames = new [] {
		"James",
		"John",
		"Robert",
		"Michael",
		"William",
		"David",
		"Richard",
		"Charles",
		"Joseph",
		"Thomas",
		"Christopher",
		"Daniel",
		"Paul",
		"Mark",
		"Donald",
		"George",
		"Kenneth",
		"Steven",
		"Edward",
		"Brian",
		"Ronald",
		"Anthony",
		"Kevin",
		"Jason",
		"Matthew",
		"Gary",
		"Timothy",
		"Jose",
		"Larry",
		"Jeffrey",
        "Syver",
        "Anders",
        "Magnus",
		"Frank",
		"Scott",
		"Eric",
		"Stephen",
		"Andrew",
		"Raymond",
		"Gregory",
		"Joshua",
		"Jerry",
		"Dennis",
		"Walter",
		"Patrick",
		"Peter",
		"Harold",
		"Douglas",
		"Henry",
		"Carl",
		"Arthur",
		"Ryan",
		"Roger",
		"Joe",
		"Juan",
		"Jack",
		"Albert",
		"Jonathan",
		"Justin",
		"Terry",
		"Gerald",
		"Keith",
		"Samuel",
		"Willie",
		"Ralph",
		"Lawrence",
		"Nicholas",
		"Roy",
		"Benjamin",
		"Bruce",
		"Brandon",
		"Adam",
		"Harry",
		"Fred",
		"Wayne",
		"Billy",
		"Steve",
		"Louis",
		"Jeremy",
		"Aaron",
		"Randy",
		"Howard",
		"Eugene",
		"Carlos",
		"Russell",
		"Bobby",
		"Victor",
		"Martin",
		"Ernest",
		"Phillip",
		"Todd",
		"Jesse",
		"Craig",
		"Alan",
		"Shawn",
		"Clarence",
		"Sean",
		"Philip",
		"Chris",
		"Johnny",
		"Earl",
		"Jimmy",
		"Antonio",
		"Danny",
		"Bryan",
		"Tony",
		"Luis",
		"Mike",
		"Stanley",
		"Leonard",
		"Nathan",
		"Dale",
		"Manuel",
		"Rodney",
		"Curtis",
		"Norman",
		"Allen",
		"Marvin",
		"Vincent",
		"Glenn",
		"Jeffery",
		"Travis",
		"Jeff",
		"Chad",
		"Jacob",
		"Lee",
		"Melvin",
		"Alfred",
		"Kyle",
		"Francis",
		"Bradley",
		"Jesus",
		"Herbert",
		"Frederick",
		"Ray",
		"Joel",
		"Edwin",
		"Don",
		"Eddie",
		"Ricky",
		"Troy",
		"Randall",
		"Barry",
		"Alexander",
		"Bernard",
		"Mario",
		"Leroy",
		"Francisco",
		"Marcus",
		"Micheal",
		"Theodore",
		"Clifford",
		"Miguel",
		"Oscar",
		"Jay",
		"Jim",
		"Tom",
		"Calvin",
		"Alex",
		"Jon",
		"Ronnie",
		"Bill",
		"Lloyd",
		"Tommy",
		"Leon",
		"Derek",
		"Warren",
		"Darrell",
		"Jerome",
		"Floyd",
		"Leo",
		"Alvin",
		"Tim",
		"Wesley",
		"Gordon",
		"Dean",
		"Greg",
		"Jorge",
		"Dustin",
		"Pedro",
		"Derrick",
		"Dan",
		"Lewis",
		"Zachary",
		"Corey",
		"Herman",
		"Maurice",
		"Vernon",
		"Roberto",
		"Clyde",
		"Glen",
		"Hector",
		"Shane",
		"Ricardo",
		"Sam",
		"Rick",
		"Lester",
		"Brent",
		"Ramon",
		"Charlie",
		"Tyler",
		"Gilbert",
		"Gene",
		"Marc",
		"Reginald",
		"Ruben",
		"Brett",
		"Angel",
		"Nathaniel",
		"Rafael",
		"Leslie",
		"Edgar",
		"Milton",
		"Raul",
		"Ben",
		"Chester",
		"Cecil",
		"Duane",
		"Franklin",
		"Andre",
		"Elmer",
		"Brad",
		"Gabriel",
		"Ron",
		"Mitchell",
		"Roland",
		"Arnold",
		"Harvey",
		"Jared",
		"Adrian",
		"Karl",
		"Cory",
		"Claude",
		"Erik",
		"Darryl",
		"Jamie",
		"Neil",
		"Jessie",
		"Christian",
		"Javier",
		"Fernando",
		"Clinton",
		"Ted",
		"Mathew",
		"Tyrone",
		"Darren",
		"Lonnie",
		"Lance",
		"Cody",
		"Julio",
		"Kelly",
		"Kurt",
		"Allan",
		"Nelson",
		"Guy",
		"Clayton",
		"Hugh",
		"Max",
		"Dwayne",
		"Dwight",
		"Armando",
		"Felix",
		"Jimmie",
		"Everett",
		"Jordan",
		"Ian",
		"Wallace",
		"Ken",
		"Bob",
		"Jaime",
		"Casey",
		"Alfredo",
		"Alberto",
		"Dave",
		"Ivan",
		"Johnnie",
		"Sidney",
		"Byron",
		"Julian",
		"Isaac",
		"Morris",
		"Clifton",
		"Willard",
		"Daryl",
		"Ross",
		"Virgil",
		"Andy",
		"Marshall",
		"Salvador",
		"Perry",
		"Kirk",
		"Sergio",
		"Marion",
		"Tracy",
		"Seth",
		"Kent",
		"Terrance",
		"Rene",
		"Eduardo",
		"Terrence",
		"Enrique",
		"Freddie",
		"Wade",
		"Austin",
		"Stuart",
		"Fredrick",
		"Arturo",
		"Alejandro",
		"Jackie",
		"Joey",
		"Nick",
		"Luther",
		"Wendell",
		"Jeremiah",
		"Evan",
		"Julius",
		"Dana",
		"Donnie",
		"Otis",
		"Shannon",
		"Trevor",
		"Oliver",
		"Luke",
		"Homer",
		"Gerard",
		"Doug",
		"Kenny",
		"Hubert",
		"Angelo",
		"Shaun",
		"Lyle",
		"Matt",
		"Lynn",
		"Alfonso",
		"Orlando",
		"Rex",
		"Carlton",
		"Ernesto",
		"Cameron",
		"Neal",
		"Pablo",
		"Lorenzo",
		"Omar",
		"Wilbur",
		"Blake",
		"Grant",
		"Horace",
		"Roderick",
		"Kerry",
		"Abraham",
		"Willis",
		"Rickey",
		"Jean",
		"Ira",
		"Andres",
		"Cesar",
		"Johnathan",
		"Malcolm",
		"Rudolph",
		"Damon",
		"Kelvin",
		"Rudy",
		"Preston",
		"Alton",
		"Archie",
		"Marco",
		"Wm",
		"Pete",
		"Randolph",
		"Garry",
		"Geoffrey",
		"Jonathon",
		"Felipe",
		"Bennie",
		"Gerardo",
		"Ed",
		"Dominic",
		"Robin",
		"Loren",
		"Delbert",
		"Colin",
		"Guillermo",
		"Earnest",
		"Lucwas",
		"Benny",
		"Noel",
		"Spencer",
		"Rodolfo",
		"Myron",
		"Edmund",
		"Garrett",
		"Salvatore",
		"Cedric",
		"Lowell",
		"Gregg",
		"Sherman",
		"Wilson",
		"Devin",
		"Sylvester",
		"Kim",
		"Roosevelt",
		"Israel",
		"Jermaine",
		"Forrest",
		"Wilbert",
		"Leland",
		"Simon",
		"Guadalupe",
		"Clark",
		"Irving",
		"Carroll",
		"Bryant",
		"Owen",
		"Rufus",
		"Woodrow",
		"Sammy",
		"Kristopher",
		"Mack",
		"Levi",
		"Marcos",
		"Gustavo",
		"Jake",
		"Lionel",
		"Marty",
		"Taylor",
		"Ellis",
		"Dallas",
		"Gilberto",
		"Clint",
		"Nicolas",
		"Laurence",
		"Ismael",
		"Orville",
		"Drew",
		"Jody",
		"Ervin",
		"Dewey",
		"Al",
		"Wilfred",
		"Josh",
		"Hugo",
		"Ignacio",
		"Caleb",
		"Tomas",
		"Sheldon",
		"Erick",
		"Frankie",
		"Stewart",
		"Doyle",
		"Darrel",
		"Rogelio",
		"Terence",
		"Santiago",
		"Alonzo",
		"Elias",
		"Bert",
		"Elbert",
		"Ramiro",
		"Conrad",
		"Pat",
		"Noah",
		"Grady",
		"Phil",
		"Cornelius",
		"Lamar",
		"Rolando",
		"Clay",
		"Percy",
		"Dexter",
		"Bradford",
		"Merle",
		"Darin",
		"Amos",
		"Terrell",
		"Moses",
		"Irvin",
		"Saul",
		"Roman",
		"Darnell",
		"Randal",
		"Tommie",
		"Timmy",
		"Darrin",
		"Winston",
		"Brendan",
		"Toby",
		"Van",
		"Abel",
		"Dominick",
		"Boyd",
		"Courtney",
		"Jan",
		"Emilio",
		"Elijah",
		"Cary",
		"Domingo",
		"Santos",
		"Aubrey",
		"Emmett",
		"Marlon",
		"Emanuel",
		"Jerald",
		"Edmond"
	};

    private static readonly string[] FemaleAvatarFilenames = {
		"Avatars/female01",
		"Avatars/female02",
		"Avatars/female03",
		"Avatars/female04",
		"Avatars/female05",
		"Avatars/female06",
		"Avatars/female07",
		"Avatars/female08",
	};

    private static readonly string[] MaleAvatarFilenames = {
		"Avatars/male01",
		"Avatars/male02",
		"Avatars/male03",
		"Avatars/male04",
		"Avatars/male05",
		"Avatars/male06",
		"Avatars/male07",
		"Avatars/male08",
	};
}