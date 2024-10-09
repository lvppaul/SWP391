import { Container } from "react-bootstrap";
import { useState } from "react";
// import AddNewBlog from '../../components/AddNewBlog/AddNewBlog'

const Blog = () => {
  const [showModalNewBlog, setShowModalNewBlog] = useState(false);

  return (
    <Container>
      <h1>Blogs</h1>

      {/* <div className="main-blog">
            <AddNewBlog 
            show = {showModalNewBlog}
            setShow = {setShowModalNewBlog}/>
        </div> */}
    </Container>
  );
};

export default Blog;
