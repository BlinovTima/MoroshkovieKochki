using System;

namespace MoroshkovieKochki
{
    public static class Animations
    {

        public static AnimationPreset GetPreset(CharacterAnimationPreset preset)
        {
            switch (preset)
            {
                case CharacterAnimationPreset.Newspaper:
                    return new AnimationPreset()
                    {
                        Idle = "idle_newspaper",
                        Hello = "idle_hello",
                        Say = "idle_newspaper_say",
                        Walk = "walk_newspaper",
                        Hit = "idle_newspaper_hit",
                    };
                
                case CharacterAnimationPreset.Box:
                    return new AnimationPreset()
                    {
                        Idle = "idle_box",
                        Hello = "idle_hello",
                        Say = "idle_box_say",
                        Walk = "walk_box",
                        Take = "idle_box_take",
                    };
                
                case CharacterAnimationPreset.Basket:
                    return new AnimationPreset()
                    {
                        Idle = "idle_basket",
                        Hello = "idle_hello",
                        Say = "idle_basket_say",
                        Walk = "walk_basket",
                        Take = "idle_basket_take",
                    };
                
                default:
                    return new AnimationPreset()
                    {
                        Idle = "idle",
                        Hello = "idle_hello",
                        Say = "idle_say",
                        Walk = "walk",
                    };
            }
           
        }
    }
}