
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga {
    class Score {

        private int scoreCount = 0;

        private Text scoreText;

        public Score(Vec2F pos, Vec2F size){
            scoreText = new Text("Score: 0", pos, size);
            scoreText.SetColor(new Vec3I(255,255,255));
        }

        public void IncrementScore(){
            scoreCount += 1;
            scoreText.SetText($"Score: {scoreCount}");
        }

        public void RenderScore(){
            scoreText.RenderText();
        }

    }
}
