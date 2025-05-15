ALTER TABLE image
DROP COLUMN image_key;

ALTER TABLE image
ADD COLUMN file_location VARCHAR(500);
