CREATE TABLE user_info (
	user_id UUID PRIMARY KEY,
	display_name VARCHAR(100),
	last_login TIMESTAMPTZ
);

CREATE TABLE journal (
	journal_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	user_id UUID NOT NULL,
	created TIMESTAMPTZ NOT NULL,
	updated TIMESTAMPTZ NOT NULL,
	title VARCHAR(200),
	CONSTRAINT fk_journal_user_info
		FOREIGN KEY(user_id)
			REFERENCES user_info(user_id)
);

CREATE TABLE journal_page (
	journal_page_id UUID PRIMARY KEY,
	journal_id INT NOT NULL,
	created TIMESTAMPTZ NOT NULL,
	updated TIMESTAMPTZ NOT NULL,
	page_number INT NOT NULL,
	title VARCHAR(200),
	contents TEXT,
	CONSTRAINT fk_journal_page_journal
		FOREIGN KEY(journal_id)
			REFERENCES journal(journal_id)
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

