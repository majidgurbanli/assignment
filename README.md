# assignment

CREATE TABLE RecordsTable (
    tpep_pickup_datetime DATETIME,
    tpep_dropoff_datetime DATETIME,
    passenger_count INT,
    trip_distance FLOAT,
    store_and_fwd_flag VARCHAR(10),
    PULocationID INT,
    DOLocationID INT,
    fare_amount DECIMAL(10, 2),
    tip_amount DECIMAL(10, 2)
);
CREATE INDEX IX_PULocationID ON RecordsTable (PULocationID);
CREATE INDEX IX_trip_distance ON RecordsTable (trip_distance);
CREATE INDEX IX_tpep_pickup_datetime ON RecordsTable (tpep_pickup_datetime);


Note: You should change connection string DatabaseOperations class line 33 to Your own connection string to work(I do not have resources to deploy it)
For larger datas I will definitely use multithreading for inserting. I will read from file in multiple threads and insert it to database.