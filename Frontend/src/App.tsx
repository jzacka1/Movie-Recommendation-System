import { useState } from "react";
import axios from "axios";

function App() {
  const [text, setText] = useState("");
  const [result, setResult] = useState<any>(null);

  const analyzeSentiment = async () => {
    try {
      // const response = await axios.post("http://127.0.0.1:8000/analyze", {
      //   text,
      // });
      // const response = await axios.post("http://localhost:8000/analyze", {
      //   text,
      // });
      const response = await axios.post("http://backend:8000/analyze", {
        text,
      });

      setResult(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div style={{ padding: "2rem", fontFamily: "Arial" }}>
      <h1>AI Sentiment Analyzer</h1>
      <textarea
        rows={5}
        cols={40}
        placeholder="Type your text here..."
        value={text}
        onChange={(e) => setText(e.target.value)}
      />
      <br />
      <button onClick={analyzeSentiment} style={{ marginTop: "1rem" }}>
        Analyze
      </button>

      {result && (
        <div style={{ marginTop: "2rem" }}>
          <h2>Result:</h2>
          <p><b>Label:</b> {result.label}</p>
          <p><b>Score:</b> {result.score.toFixed(3)}</p>
        </div>
      )}
    </div>
  );
}

export default App;
