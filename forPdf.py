import os
from PIL import Image
import img2pdf

def convert_to_pdf():
    """将图片转换为PDF"""
    images = []
    # 按页码顺序获取图片文件
    for page in range(start_page, end_page + 1):
        img_path = os.path.join(output_dir, f"slide_{page:03d}.png")
        if not os.path.exists(img_path):
            print(f"文件不存在: {img_path}")
            continue
            
        try:
            # 打开并处理图片
            with Image.open(img_path) as img:
                # 如果是RGBA模式，转换为RGB
                if img.mode == 'RGBA':
                    # 创建白色背景
                    background = Image.new('RGB', img.size, (255, 255, 255))
                    # 将RGBA图片粘贴到白色背景上
                    background.paste(img, mask=img.split()[3])  # 使用alpha通道作为mask
                    img = background
                elif img.mode != 'RGB':
                    img = img.convert('RGB')
                
                # 保存处理后的图片到临时文件
                temp_path = os.path.join(output_dir, f"temp_{page:03d}.png")
                img.save(temp_path, 'PNG')
                images.append(temp_path)
                print(f"处理完成: {img_path}")
        except Exception as e:
            print(f"处理图片时出错 {img_path}: {str(e)}")
            continue

    if not images:
        print("没有找到可处理的图片")
        return

    try:
        # 转换为PDF
        with open(output_pdf, "wb") as f:
            pdf_bytes = img2pdf.convert(images)
            f.write(pdf_bytes)
        print(f"PDF已成功生成：{output_pdf}")
        
        # 清理临时文件
        for temp_img in images:
            try:
                os.remove(temp_img)
            except:
                pass
    except Exception as e:
        print(f"生成PDF时出错: {str(e)}") 