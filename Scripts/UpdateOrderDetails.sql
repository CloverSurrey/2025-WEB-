-- 首先删除现有的主键约束（如果存在）
IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_order_details')
BEGIN
    ALTER TABLE order_details DROP CONSTRAINT PK_order_details;
END

-- 添加自增ID列
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('order_details') AND name = 'id')
BEGIN
    ALTER TABLE order_details ADD id INT IDENTITY(1,1) NOT NULL;
END

-- 设置新的主键
ALTER TABLE order_details ADD CONSTRAINT PK_order_details PRIMARY KEY (id);

-- 为order_id和product_id添加外键约束（如果需要）
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_order_details_order')
BEGIN
    ALTER TABLE order_details ADD CONSTRAINT FK_order_details_order 
    FOREIGN KEY (order_id) REFERENCES [order](order_id);
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_order_details_product')
BEGIN
    ALTER TABLE order_details ADD CONSTRAINT FK_order_details_product 
    FOREIGN KEY (product_id) REFERENCES product(product_id);
END 