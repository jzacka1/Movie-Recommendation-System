from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from transformers import pipeline

# Initialize FastAPI app
app = FastAPI()

# origins = [
#     "http://localhost:5173/",
#     "http://localhost:8000/"
# ]

# app.add_middleware(
#     CORSMiddleware,
#     allow_origins=origins,
#     allow_credentials=True,
#     allow_methods=["*"],
#     allow_headers=["*"]
# )

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"]
)

# Load sentiment analysis pipeline
sentiment_pipeline = pipeline("sentiment-analysis")

# Request model
class TextInput(BaseModel):
    text: str

@app.post("/analyze")
def analyze_text(input: TextInput):
    result = sentiment_pipeline(input.text)[0]
    return {"label": result["label"], "score": result["score"]}
