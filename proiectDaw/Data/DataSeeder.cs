using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using proiectDaw.Models;

namespace proiectDaw.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(ApplicationDbContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        private Recipe createRecipe(string name, string description, int kcal, string difficulty, int time,
            string mealType, string[] steps, string[] ingredientsNames, string[] ingredientsQuantity,
            string[] ingredientsTypes
        )
        {
            var recipe = new Recipe
            {
                Name = name, Description = description, Kcal = kcal, Difficulty = difficulty, Time = time,
                MealType = mealType, Steps = steps
            };
            recipe.Ingredients = new List<Ingredient>();


            for (int i = 0; i < ingredientsNames.Length; i++)
            {
                var ingredient = new Ingredient
                    {Name = ingredientsNames[i], Quantity = ingredientsNames[i], Recipe = recipe, Type = ingredientsTypes[i]};
                recipe.Ingredients.Add(ingredient);
            }

            return recipe;
        }

        public void Seed()
        {
            var recipeDB = _context.Recipes.Include(e => e.Ingredients);

            // var recciipe = recipeDB.Where(rcp => rcp.Id == 3);
            // Console.WriteLine(recciipe.First().Ingredients[0].Name);

            Console.WriteLine("Seeding database");
            _logger.LogInformation("Seeding database");

            // Cleaning the db
            _context.Recipes.RemoveRange(_context.Recipes);
            _context.Ingredients.RemoveRange(_context.Ingredients);
            _context.SaveChanges();
            
            var recipe1 = createRecipe("Simple Carbonara",
                "Humble ingredients—eggs, noodles, cheese, and pork—combine to create glossy, glorious pasta carbonara. It's the no-food-in-the-house dinner of our dreams.",
                400, "Easy", 40, "Pasta",
                new string[]
                {
                    "Heat 6 qt. water in a large pot over high. When water starts to steam, add 3 Tbsp. salt and cover pot with a lid (this will bring water to a boil faster).",
                    "While you are waiting on the water, do a little prep. Remove 4 oz. guanciale from packaging and cut into about 1x¼' strips. Finely grate 2 oz. cheese and set aside one-quarter of cheese for later.",
                    "Whisk 4 egg yolks and 2 whole eggs in a medium bowl until no streaks remain, then stir in remaining grated cheese. Add several cranks of pepper and set aside.",
                    "Working next to pot, heat 2 Tbsp. oil in a large Dutch oven or other heavy pot over medium. Add guanciale and cook, stirring occasionally, until crisp around the edges, 7–10 minutes.",
                    "Remove pot from heat. Using a wooden spoon, fish out guanciale and transfer to a small bowl. Pour fat into a heatproof measuring cup, then add back about 3 Tbsp. to pot. Discard any remaining fat.",
                    "Cook 1 lb. pasta in boiling water, stirring occasionally, 2 minutes shy of package instructions. Just before pasta is finished, scoop out 1¾ cups pasta cooking liquid with same heatproof measuring cup.",
                    "Add 1 cup reserved pasta cooking liquid to Dutch oven and bring to a boil over medium-high. Drain pasta in a colander, then transfer to Dutch oven.",
                    "Cook pasta, stirring constantly and vigorously, until al dente and water is reduced by about half, about 2 minutes. Remove pot from heat.",
                    "Whisk ¼ cup pasta cooking liquid into reserved egg mixture, then very slowly stream into Dutch oven, stirring constantly, until cheese is melted and egg is thickened to form a glossy sauce. Season with salt, if needed. Thin sauce with remaining ½ cup pasta cooking liquid, adding a tablespoonful at a time, until it's the consistency of heavy cream (you most likely won’t use all of it).",
                    "Mix in guanciale and divide pasta among bowls. Top with pepper and reserved cheese."
                },
                new string[]
                {
                    "kosher salt, plus more",
                    "oz. guanciale (salt-cured pork jowl), pancetta (Italian bacon), or bacon",
                    "Parmesan", "large egg yolks", "large eggs", "Freshly ground black pepper",
                    "extra-virgin olive oil", "lb. spaghetti, bucatini, or rigatoni"
                },
                new string[]
                {
                    "3 Tbsp", "4oz", "4", "2", "1 tsp", "Tbsp", "1 lb"
                },
                new string[]
                {
                    "spice", "meat", "cheese", "egg", "egg", "spice", "oil", "pasta"
                });
            _context.Recipes.Add(recipe1);
            var recipe2 = createRecipe("Vegetarian Chili With Lots of Fritos",
                "Quick, spicy, filling, and made mostly of pantry ingredients, this chili/tortilla soup/frito pie mash-up is calling your name. (Shhh. Listen closely.) Garnish it with the toppings of your choice: We like avocado, sour cream, and chopped white onion, but you can add shredded cheese, pickled or fresh jalapeños, and/or sliced scallions or radishes. You can use any tortilla chips you like—they add body and flavor to the soup itself and go on top for extra oomph. You could also fry or bake your own stale tortillas, cut into strips, but if we're being honest, salty, crunchy Fritos will probably taste better. ",
                200, "Easy", 20, "Vegetarian",
                new string[]
                {
                    "Prep your ingredients: First, finely chop 1 large white onion. Cut in half from top to bottom. Trim top, then peel onion skin and first tough layer; discard. Leave root end on. Make slices parallel to cutting board, starting close to the board and moving upward. Then make slices lengthwise from top of onion to root end. Slice across onion, again from top to bottom, and you’ll come away with nice little cubes. Run your knife through once more if any pieces are too big. You should have about 2 cups chopped onions. Set aside 2 Tbsp. for serving.",
                    "Smash, peel, and finely chop 6 garlic cloves. Separate leaves from stems from ½ bunch cilantro. Coarsely chop leaves; set aside for serving. Finely chop stems. Using a fork, pull 1 chipotle chile from can, then finely chop until a paste forms. Transfer to a small bowl, along with 1 tsp. adobo sauce from can. Add ¼ cup tomato paste. Drain and rinse 2 15-oz. cans pinto beans.",
                    "Heat ¼ cup extra-virgin olive oil in a large saucepan or Dutch oven over medium-high. Add onion, garlic, cilantro stems, and 2 tsp. chili powder and cook, stirring frequently, until onion is starting to soften, 5–6 minutes. Add tomato paste mixture and cook, stirring frequently, until brick red and starting to stick to bottom of pan, 2–3 minutes.",
                    "Add 1 28-oz. can fire-roasted tomatoes and bring to a simmer. Cook, stirring frequently, until tomatoes are cooked down and slightly darker in color, 8–10 minutes.",
                    "Add beans, 1 Tbsp. Diamond Crystal or 2 tsp. Morton kosher salt, and 5 cups water. Bring to a boil over high heat, then reduce heat to medium-low and simmer uncovered, stirring occasionally, until soup has reduced slightly and flavors have melded, 30–35 minutes. Mash about one-quarter of beans with the back of a wooden spoon or potato masher—this will make the soup thicker and creamier. Stir in 1 cup Fritos (or 1 cup crushed tortilla chips) and cook, stirring occasionally, until softened, 1–2 minutes. Squeeze in juice of 1 lime half, then taste for salt (you may need to add as much as 2 tsp.).",
                    "Cut remaining 1 lime half into 4 wedges. Slice 1 avocado in half, carefully remove pit, then crosshatch flesh in its shell. Scoop out flesh with a spoon.",
                    "Divide soup among bowls. Top with ½ cup sour cream, avocado, reserved cilantro leaves and chopped onion, and remaining ½ cup Fritos. Season with pepper.",
                },
                new string[]
                {
                    "large white onion", "garlic cloves", "bunch cilantro", "chipotle chiles in adobo", "adobo sauce",
                    "cup tomato paste", "15-oz. cans pinto beans", "extra-virgin olive oil", "chili powder",
                    "fire-roasted tomatoes", "Diamond Crystal", "salty tortilla chips", "lime", "avocado", "sour cream",
                    "black pepper"
                },
                new string[]
                {
                    "1", "6", "1/2", "1", "1 tsp", "1/4", "2", "1/4", "2", "1", "1 Tbsp", "1 1/2", "1", "1", "1/2",
                    "1 tsp"
                },
                new string[]
                {
                    "vegetable", "vegetable", "vegetable", "vegetable", "sauce", "vegetable", "vegetable", "oil", "spice", "vegetable", "spice", "misc", "vegetable", "fruit", "sauce", "spice"
                });

            _context.Recipes.Add(recipe2);
            var recipe3 = createRecipe("Tahini Billionaire Bars",
                "For the seventh recipe in the Basically Guide to Better Baking, we took the millionaire bar—shortbread plus caramel plus chocolate (think of it like an oversized Twix)—and gave it an upgrade with sesame seeds and tahini. The shortbread is tender, the filling is gooey, and the chocolate ties it all together. Be sure to give the tahini a good stir with a butter knife or mini offset spatula before you whisk it into the butterscotch—it has a tendency to separate. Slice the finished bars into pieces using a serrated knife so that you can saw through the layers rather than smoosh them. If you're still having trouble, just stick them in the freezer until the butterscotch is firm enough to easily slice. More questions? Head to our forum where we're eager to help. ",
                800, "Medium", 40, "Desert",
                new string[]
                {
                    "Place a rack in middle of oven; preheat to 350°. Lightly coat a 9x9” or 8x8” baking pan, preferably metal, with nonstick cooking oil spray or vegetable oil. Line with parchment paper, leaving overhang on all sides. (If you have a precut sheet of parchment, simply slice it in half crosswise, then overlap the two halves to line the pan.)",
                    "Whisk 1 cup (125 g) all-purpose flour, ¾ cup (83 g) powdered sugar, ⅓ cup toasted sesame seeds, and ¾ tsp. Diamond Crystal or ½ tsp. Morton kosher salt in a medium bowl. Cut ½ cup (1 stick) chilled unsalted butter into ½” pieces and add to dry ingredients; toss to coat. Using your fingers, work in butter until pieces are about pea-size.",
                    "Separate 2 large eggs—the easiest way to do this is to crack open eggs one at a time and gently pour into a (clean) hand, letting whites fall through your fingers either into the sink or a small bowl while trapping the yolk. Add yolks to bowl with flour mixture and mix in with a rubber spatula or wooden spoon. Dough should hold together when squeezed in your hand.",
                    "Scrape dough into prepared pan and gently press into an even layer (lightly flour your hands if needed). Prick dough with a fork in several spots (this will help it stay flat as it bakes). Lightly flour the fork if it's sticking to the dough.",
                    "Bake shortbread until golden brown, 24–28 minutes (or up to 35 minutes if using a glass dish). Let cool slightly.",
                    "While the shortbread is cooling, make the tahini butterscotch. Cut remaining ½ cup (1 stick) chilled unsalted butter into 8 pieces. Heat butter and 1¼ cups (packed; 250 g) light brown sugar, whisking constantly, in a medium heavy saucepan over medium until butter is melted and sugar is dissolved, about 3 minutes. Whisk in ¾ cup heavy cream and 2 tsp. Diamond Crystal or 1 tsp. Morton kosher salt. Bring to a boil and cook, whisking constantly, until bubbling becomes less frequent and butterscotch is thick enough to coat a spoon, about 5 minutes. (When in doubt, err on the side of giving the mixture another 30 seconds—it's better if it's a little too firm than too runny.)",
                    "" +
                    "Remove from heat, add ½ cup tahini and 1 tsp. vanilla extract and whisk until incorporated and smooth. Pour butterscotch over shortbread and tilt pan to distribute evenly if necessary. Chill until set, 30–40 minutes.",
                    "Coarsely chop 6 oz. bittersweet chocolate (65%–75% cacao) and place in a microwave-safe bowl. Melt in a microwave in 40-second increments, stirring between bursts. (Alternatively, place chocolate in a heatproof bowl set over a saucepan of gently simmering water a.k.a. a double boiler; do not let bowl touch water. Heat, stirring often, until melted and smooth.) Pour over butterscotch and spread evenly to edges of pan with a small offset spatula or rubber spatula; sprinkle with toasted sesame seeds. Chill until chocolate is firm, 1–2 hours.",
                    "Using parchment paper overhang, pull bars out of pan and set on a cutting board; remove parchment. Using a serrated knife (a chef's knife will smoosh these), slice into 25–36 pieces (5–6 cuts in each direction) with a sawing motion. If you're having trouble slicing, feel free to freeze the bars for 20–30 minutes, which will harden the caramel and ease the process.",
                    "Do ahead: Bars can be made 4 days ahead. Store airtight at in the fridge."
                },
                new string[]
                {
                    "vegetable oil", "all-purpose flour", "powdered sugar", "toasted sesame seeds", "Diamond Crysta",
                    "chilled unsalted butter", "large eggs", "light brown sugar", "heavy cream", "tahini",
                    "vanilla extract", "bittersweet chocolate"
                },
                new string[] {"125 g", "83 g", "1/3 cup", "2 3/4 cup", "1 cup", "3/4 cup", "1 cup", "1 tsp", "6 oz"},
                new string[] {"oil", "cereal", "spice", "spice", "spice", "misc", "egg", "spice", "misc", "misc", "misc", "sweet"});
            _context.Recipes.Add(recipe3);

            var recipe4 = createRecipe("Classic Chicken Noodle Soup",
                "There are tons of shortcuts for chicken noodle soup, but this time we're not cutting corners—this is the long game! This version is about as classic (and as comforting) as they come, using a whole chicken—bones, skin, and all—to lend flavor and body to the broth. The key is to treat the breasts and legs differently: The breasts need to be pulled early so they don't overcook and dry out, whereas the legs require a long simmer to become incredibly tender. We used ditalini here, but feel free to use any small quick-cooking pasta you have! We wouldn’t be mad about orzo or ABCs either.",
                210, "Medium", 80, "Soup",
                new string[]
                {
                    "Season one 3–4-lb. chicken all over with 4 tsp salt.",
                    "Time to do some prep work: Cut 2 medium onions into quarters. (There's no need to remove the onion skins, which lend the broth a golden hue, but you can if you'd like.) Peel 4 medium carrots. Coarsely chop 2; set remaining 2 aside. Coarsely chop 2 celery stalks. Cut 2 heads of garlic in half crosswise.",
                    "Combine chicken, cut vegetables, 1 Tbsp. black peppercorns, and 2 dill sprigs in a large pot. Cover with 14 cups cold water and bring to a simmer over medium-high heat. Cook, reducing heat as needed to maintain a simmer and using a large spoon to skim off any foam that rises to surface of pot, until an instant-read thermometer inserted into thickest part of breast registers 155°, 20–25 minutes.",
                    "Using tongs, carefully lift whole chicken out of pot and transfer to a cutting board. Let rest until cool enough to handle.",
                    "Arrange chicken breast side up. Grab a wing and pull it outward so you can see where it attaches to the body. Using a sharp boning or chef’s knife, cut through the joint to separate wing from breast (if you hit bone, you’re in the wrong spot; pull the wing out farther to help you get to the place where the joint meets the socket). Remove wing; repeat on the other side.",
                    "Cut through skin connecting 1 leg to carcass. Pull leg back until ball joint pops out of its socket; cut through the joint to separate the leg. Repeat on the other side.",
                    "Now for the breasts: Cut along left side of breastbone (which runs right down the center of the breast). Angling your knife, cut breast meat away from carcass. Repeat this process, cutting down along the right side of the breast bone for the remaining breast.",
                    "Pull off and discard any skin from legs and breasts (no need to discard skin of the wings). Return legs, wings, and what remains of the carcass to pot with vegetables. (You should now have only the chicken breasts remaining on your cutting board.)",
                    "Continue to simmer soup, occasionally skimming fat that rises to the top with large spoon, until reduced by an inch or two and very full-flavored, about 40 minutes.",
                    "While soup simmers, shred cooled chicken breasts with 2 forks into bite-sized pieces.",
                    "Thinly slice remaining 2 celery stalks crosswise. Cut remaining 2 medium carrots into ½ diagonal pieces. Finely chop enough dill to yield ¼ cup.",
                    "Transfer 2 chicken legs to cutting board to cool. Set a fine-mesh sieve over another large pot. Strain soup into second pot, discarding bones, carcass, wings, and vegetables.",
                    "Bring broth to a boil over medium-high heat. Add 6 oz. ditalini and stir once. Cook 5 minutes.",
                    "While ditalini cooks, shred meat off 2 chicken legs; discard bones.",
                    "Add shredded chicken and sliced carrots and celery to pot and cook until pasta is cooked through and vegetables are tender but not mushy, 4–5 minutes longer.",
                    "Remove pot from heat. Stir in dill. Season well with salt (it’s going to take a lot!) and pepper.",
                    "Divide soup among bowls. Top with more pepper.",
                },
                new string[]
                {
                    "whole chicken", "kosher salt", "medium onions", "medium carrots", "celery stalks",
                    "heads of garlic", "black peppercorns", "small bunch dill", "ditalini",
                    "Freshly ground black pepper"
                },
                new string[] {"1 3-4 lb", "4 tsb", "2", "4", "4", "2", "1", "1/2", "6 os", "1 tsb"},
                new string[] {"meat", "spice", "vegetable", "vegetable", "vegetable", "vegetable", "spice", "spice", "misc", "spice"});

            _context.Recipes.Add(recipe4);
            _context.SaveChanges();
            //Seeding the db
            Console.WriteLine("Database seeded");
            _logger.LogInformation($"Database seeded");
        }
    }
}