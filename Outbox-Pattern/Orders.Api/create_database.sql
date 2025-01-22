-- Create Orders table if not exists
CREATE TABLE IF NOT EXISTS orders(
  id UUID Primary KEY,
  customer_name VARCHAR(255) NOT NULL,
  product_name VARCHAR(255) NOT NULL,
  quantity INTEGER NOT NULL,
  total_price DECIMAL(18,2) NOT NULL,
  order_date TIMESTAMP WITH TIME ZONE NOT NULL
  );

-- Create Outbox table if it does not exists
CREATE TABLE IF NOT EXISTS outbox_message(
	id UUID PRIMARY KEY,
	type VARCHAR(255) NOT NULL,
	content_JSONB NOT NULL,
	occured_on_utc TIMESTAMP WITH TIME ZONE NOT NULL,
	processed_on_utc TIMESTAMP WITH TIME ZONE  NULL,
	error TEXT NULL

);