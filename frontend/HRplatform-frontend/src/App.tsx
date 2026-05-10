import { useEffect, useState } from "react";

export default function App() {
  const [text, setText] = useState("loading...");

  useEffect(() => {
    async function test() {
      try {
        const res = await fetch("/api/Skills")
        setText(`api reachable, status=${res.status}`);
      } catch (e: any) {
        setText(`error: ${e.message ?? e}`);
      }
    }
    test();
  }, []);

  return <div>{text}</div>;
}