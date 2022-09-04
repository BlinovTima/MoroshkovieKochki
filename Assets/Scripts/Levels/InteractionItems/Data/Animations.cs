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
                        ThinkStart = "idle_newspaper_think_one",
                        ThinkLoop = "idle_newspaper_think_two",
                        ThinkFinish = "idle_newspaper_think_three",
                        No = "idle_newspaper_think_no",
                    };
                
                case CharacterAnimationPreset.Box:
                    return new AnimationPreset()
                    {
                        Idle = "idle_box",
                        Hello = "idle_hello",
                        Say = "idle_box_say",
                        Walk = "walk_box",
                        Take = "idle_box_take",
                        ThinkStart = "idle_box_think_one",
                        ThinkLoop = "idle_box_think_two",
                        ThinkFinish = "idle_box_think_three",
                        No = "idle_box_think_no",
                    };
                
                case CharacterAnimationPreset.Basket:
                    return new AnimationPreset()
                    {
                        Idle = "idle_basket",
                        Hello = "idle_hello",
                        Say = "idle_basket_say",
                        Walk = "walk_basket",
                        Take = "idle_basket_take",
                        ThinkStart = "idle_basket_think_one",
                        ThinkLoop = "idle_basket_think_two",
                        ThinkFinish = "idle_basket_think_three",
                        No = "idle_basket_think_no",
                    };
                
                default:
                    return new AnimationPreset()
                    {
                        Idle = "idle",
                        Hello = "idle_hello",
                        Say = "idle_say",
                        Walk = "walk",
                        ThinkStart = "idle_think_one",
                        ThinkLoop = "idle_think_two",
                        ThinkFinish = "idle_think_three",
                        No = "idle_think_no",
                    };
            }
           
        }
    }
}