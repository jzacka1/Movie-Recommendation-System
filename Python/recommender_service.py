from fastapi import FastAPI
import pandas as pd
from sklearn.metrics.pairwise import cosine_similarity

app = FastAPI()

# Load dataset
movies = pd.read_csv("movies.csv")   # MovieId, Title, Genres
ratings = pd.read_csv("ratings.csv") # UserId, MovieId, Rating

# Build user-item matrix
user_item_matrix = ratings.pivot_table(index='UserId', columns='MovieId', values='Rating').fillna(0)
similarity_matrix = cosine_similarity(user_item_matrix)

def recommend_movies(user_id, n=5):
    user_index = user_id - 1
    sim_scores = list(enumerate(similarity_matrix[user_index]))
    sim_scores = sorted(sim_scores, key=lambda x: x[1], reverse=True)
    top_users = [u for u, score in sim_scores[1:6]]

    recommended_movies = ratings[ratings['UserId'].isin(top_users)].groupby('MovieId').mean()['Rating']
    top_movies = recommended_movies.sort_values(ascending=False).head(n).index
    return movies[movies['MovieId'].isin(top_movies)].to_dict(orient="records")

@app.get("/recommend/{user_id}")
def get_recommendations(user_id: int, n: int = 5):
    return recommend_movies(user_id, n)
