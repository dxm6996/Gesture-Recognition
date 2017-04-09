namespace GestureRecognizer {
    public class Result {

        public string Name { get; set; }

     
        public float Score { get; set; }


		public Result() {
			this.Name = "No match";
			this.Score = 0;
		}


        public Result(string name, float score) {
            this.Name = name;
            this.Score = score;
        }

		public override string ToString() {
			return this.Name + "; " + this.Score;
		}

    } 
}
