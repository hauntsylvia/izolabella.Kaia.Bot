using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Constants.Exploration
{
    public class ExplorationStrings
    {
        public static class NotebookEvent
        {
            private static string[] PossibleMessages => new string[]
            {
                "a butterfly! . . oh, u've killed it. at least now u have a notebook to write down ur regrets.",
                "u've found a notebook! it may as well be free therapy.",
                "after approaching an out-of-place color within the meadow, u find a notebook. it has clearly been through a lot, but it doesn't seem as though anyone is coming back for it.",
            };

            public static string Message => PossibleMessages[new Random().Next(0, PossibleMessages.Length)];
        }

        public static class RoseEvent
        {
            private static string[] PossibleMessages => new string[]
            {
                "u've found a rose. be careful picking it up!",
                "roses r fun! so u decide to take it with u by killing it.",
                "a rose!"
            };

            public static string Message => PossibleMessages[new Random().Next(0, PossibleMessages.Length)];
        }

        public static class DeadFingerEvent
        {
            private static string[] PossibleMessages => new string[]
            {
                ". . . let's not ask too many questions."
            };

            public static string Message => PossibleMessages[new Random().Next(0, PossibleMessages.Length)];
        }
        public static class CigaretteKickNutAndBoltEvent
        {
            private static string[] PossibleMessages => new string[]
            {
                "u light an old cigarette u found out of slight impulse, whilst also kicking random bits of building material on the ground.",
                "u find a cigarette laying in ur bag that u forgot about. bits of building material catch ur eye, so u stuff them in ur bag as well."
            };

            public static string Message => PossibleMessages[new Random().Next(0, PossibleMessages.Length)];
        }
    }
}
