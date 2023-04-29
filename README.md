# Motel Management
Dự án quản lý nhà trọ - môn học PRN221
## Mô tả : 
 ### Quản lý, Booking, Passing 
 ### Công nghệ : C#, Razor Page, Entity FW, HTML, Bootstrap 
 ### Tổng thể luồng : 
   #### Home Page : 
   ![1](https://user-images.githubusercontent.com/99120557/235279948-fa4a3034-5ba2-47c6-88a6-02f8568e7ac2.JPG)
   #### Người dùng chưa phải chủ của phòng thì có thể booking : 
   ![2](https://user-images.githubusercontent.com/99120557/235282724-63a46ea8-d86a-4af5-aa8b-8f3379c5a40b.JPG)
   #### Sau khi đã booking thì có thể xem trạng thái của phòng mình vừa booking
   ![3 Có thể hủy đăng kí](https://user-images.githubusercontent.com/99120557/235282756-1495ba2d-1c54-4e8f-adf6-6bfdb2ac10b6.JPG)
   #### Có thể hủy và đăng kí lại ( ghi lại lịch sử booking ) 
   ![4 Hủy đăng kí và đăng kí lại](https://user-images.githubusercontent.com/99120557/235282787-6eb9ce9d-3d9e-4b02-a74a-ff9bba31c168.JPG)
   #### Giao diện admin
   ![5 Giao diện admin](https://user-images.githubusercontent.com/99120557/235282819-9aa63340-1e0b-4a68-8083-d5bd2e51769a.JPG)
   #### Danh sách booking theo phòng 
   ![6 Danh sách booking](https://user-images.githubusercontent.com/99120557/235282842-f2479410-14b9-4623-9949-0f101c8d2c50.JPG)
   #### Admin có thể hẹn ngày gặp để 2 bên xem phòng và thỏa thuận hợp đồng ( hẹn all user và được set từng user ) 
   ![7 hẹn](https://user-images.githubusercontent.com/99120557/235282861-56057de3-4700-4ea6-baa7-a88248e14696.JPG)
   #### Bên user vừa đăng kí phòng đó nếu được set ngày gặp thì có thể xem 
   ![8 Hiện thị ngày gặp](https://user-images.githubusercontent.com/99120557/235282891-9bea8ba5-1512-4d74-8665-684f35c40ba1.JPG)
   #### Admin set 1 user thành chủ của phòng đó 
   ![9 Set member](https://user-images.githubusercontent.com/99120557/235282907-f130a310-1244-4c41-a6f8-e0f8a6e7cc35.JPG)
   #### Sau khi set thì tất cả user còn lại được thông báo bị reject và hủy ngày gặp
   ![11 Các user bị từ chối](https://user-images.githubusercontent.com/99120557/235283368-0ede73b5-a5b7-4013-837d-fa47b557eb39.JPG)
   #### User được set thì sẽ hiện lên mục quản lý phòng 
   ![10 Đã hiện thị mục quản lý phòng](https://user-images.githubusercontent.com/99120557/235282944-bd4bc86a-e99d-4d39-b5fc-ea0bddb187cb.JPG)
   #### My room của người dùng 
   ![12 My Room](https://user-images.githubusercontent.com/99120557/235282952-e84f35a4-cca9-4be9-a77c-488e080702b8.JPG)
   #### Mỗi tháng admin sẽ tạo hóa đơn tiền ( giao diện tạo và xuất hóa đơn ) 
   ![13 Tạo Hóa Đơn](https://user-images.githubusercontent.com/99120557/235282969-60f2541e-ddf6-46cd-ae07-faedb92e44f7.JPG)
   #### Tạo hóa đơn có thể import từ excel vào 
   ![14 Mẫu Excel](https://user-images.githubusercontent.com/99120557/235282995-c7de9f3d-9695-4c9e-ac39-939966614ff9.JPG)
   ![15 Tạo](https://user-images.githubusercontent.com/99120557/235283002-1e11b9cd-f803-46ad-a868-4f20748139e6.JPG)
   #### User vào mục hóa đơn để thanh toán ( gửi ảnh đã gửi tiền cho bên admin check )
   ![16 Bên User](https://user-images.githubusercontent.com/99120557/235283038-3dccc397-24fd-4e29-88e2-c79ce691cd15.JPG)
   ![17 Đóng tiền phòng](https://user-images.githubusercontent.com/99120557/235283437-2188f8f9-b135-42fb-9677-2c565e1bbe15.JPG)
   #### Admin sẽ xác nhận đã đóng hay chưa
   ![18 Bên Admin Xác nhận đã nộp chưa](https://user-images.githubusercontent.com/99120557/235283061-43200119-63b2-4b98-a8c9-88e757d50984.JPG)
   #### User hiện thị được lịch sử và xem trạng thái hóa đơn của phòng
   ![19 Bên User đã được acpt hóa đơn](https://user-images.githubusercontent.com/99120557/235283088-ac5729bf-50a1-4c7d-8753-8073998c526d.JPG)
   #### Admin có thể xuất danh sách hóa đơn của tháng từ web ra excel nếu cần 
   ![20 Xuất Hóa Đơn](https://user-images.githubusercontent.com/99120557/235283136-6602dc24-5db6-4608-8fb2-19f92fadf24f.JPG)
   #### User có thể gia hạn hợp đồng 
   ![21 Gia hạn hợp đồng](https://user-images.githubusercontent.com/99120557/235283158-e0c7e31d-e45b-4633-a9a5-c8ecc7baf092.JPG)
   #### User có thể nhượng phòng ( Bắt đầu luồng nhượng phòng ) 
   ![22 Nhượng phòng](https://user-images.githubusercontent.com/99120557/235283182-d80afc82-585c-4ef8-8028-72d9da93f968.JPG)
   #### Sau khi nhượng thì phòng ở home page sẽ đổi trạng thái "Cần Pass" để người khác có thể booking
   ![23 Phòng đã hiện trạng thái cần pass](https://user-images.githubusercontent.com/99120557/235283235-788a5fda-6a78-49f3-897e-de034a4a3e36.JPG)
   #### Lúc này người pass phòng là chủ của phòng sẽ làm việc với người đặt phòng ( danh sách booking passing trong my room )
   ![24 Danh sách passing](https://user-images.githubusercontent.com/99120557/235283259-79d68d71-aae8-4041-9be8-d931c63ef7b7.JPG)
   #### Luồng sẽ tương tự như bên booking - chủ phòng sẽ là người hẹn gặp và xem phòng( không phải admin ) 
   #### Sau khi chủ phòng đồng ý nhượng cho một user nào dó thì user này sẽ được gửi đến admin confirm lại lần nữa
   ![25 Bên Admin cf lại](https://user-images.githubusercontent.com/99120557/235283304-d26c42c8-789c-4434-9a9f-fbcb3f150e37.JPG)
   #### Sau khi admin đồng ý thì phòng sẽ được đổi chủ !
   
   
# Don't pull our code for your assignment. You will get 0 for your cheating action.
# 2m contact me :>
