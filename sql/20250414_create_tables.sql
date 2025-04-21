CREATE TABLE user_info (
	user_id UUID PRIMARY KEY
);

CREATE TABLE notebook (
	notebook_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	user_id UUID NOT NULL,
	created TIMESTAMPTZ NOT NULL,
	updated TIMESTAMPTZ NOT NULL,
	title VARCHAR(200),
	CONSTRAINT fk_notebook_user_info
		FOREIGN KEY(user_id)
			REFERENCES user_info(user_id)
);

CREATE TABLE notebook_page (
	notebook_page_id UUID PRIMARY KEY,
	notebook_id INT NOT NULL,
	created TIMESTAMPTZ NOT NULL,
	updated TIMESTAMPTZ NOT NULL,
	page_number INT NOT NULL,
	title VARCHAR(200),
	contents TEXT,
	CONSTRAINT fk_notebook_page_notebook
		FOREIGN KEY(notebook_id)
			REFERENCES notebook(notebook_id)
);

CREATE TABLE image (
	image_id UUID PRIMARY KEY,
	user_id UUID NOT NULL,
	created TIMESTAMPTZ NOT NULL,
	updated TIMESTAMPTZ NOT NULL,
	image_key VARCHAR(20) NOT NULL,
	alt_text TEXT,
	CONSTRAINT fk_image_user_info
		FOREIGN KEY(user_id)
			REFERENCES user_info(user_id)
);

