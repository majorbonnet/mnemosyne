ALTER TABLE page
ADD COLUMN search_text TSVECTOR
    GENERATED ALWAYS AS (to_tsvector('english', title || ' ' || contents)) STORED;

ALTER TABLE notebook
ADD COLUMN search_text TSVECTOR
    GENERATED ALWAYS AS (to_tsvector('english', title)) STORED;