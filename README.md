# Second Brain – AI Memory System

A privacy-first, temporal, multimodal AI system that allows users to ingest
documents, audio, and web content, then query their personal knowledge base
using natural language.

## Why This Project
This prototype focuses on system architecture and reasoning over time,
not just conversational AI. The core goal is to enable accurate answers to
questions like:
- "What were the key concerns discussed in last Tuesday’s meeting?"
- "Summarize the article I saved about quantum computing."

## Architecture Overview
- ASP.NET Core 8 backend
- PostgreSQL + pgvector for metadata, vectors, and full-text search
- Background workers for async ingestion
- Hybrid retrieval (vector + keyword + temporal)
- LLM-based synthesis (RAG)

See full system design: `/docs/system-design.md`

## Key Design Decisions
- Hybrid retrieval beats vector-only search
- Temporal queries rely on `observed_at`, not ingestion time
- PostgreSQL chosen for simplicity, correctness, and time-based queries
- Privacy-first design with local-first extensibility

## Implemented Features
- Audio ingestion and transcription
- Document ingestion (PDF / Markdown)
- Chunking and vector indexing
- Time-aware retrieval
- Question answering via LLM

## Running the Project (Local)
```bash
docker-compose up
