import React, { useState, useEffect } from 'react';
import { Form, Button } from 'react-bootstrap';
import { getComments, addComment } from '../../API/AxiosConfig';

const CommentSection = ({ blogId }) => {
    const [comments, setComments] = useState([]);
    const [newComment, setNewComment] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchComments();
    }, []);

    const fetchComments = async () => {
        try {
            const data = await getComments(blogId);
            setComments(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching comments:', error);
            setLoading(false);
        }
    };

    const handleAddComment = async (e) => {
        e.preventDefault();
        const token = 'your-auth-token'; // Replace with actual token
        const comment = {
            blogId,
            commentText: newComment,
            userId: 'currentUserId', // Replace with actual user ID
            DateCreate: new Date().toISOString(),
        };
        try {
            const addedComment = await addComment(token, comment);
            setComments([addedComment, ...comments]);
            setNewComment('');
        } catch (error) {
            console.error('Error adding comment:', error);
        }
    };

    return (
        <div className="comment-section">
            <h3>Comments</h3>
            {loading ? (
                <p>Loading comments...</p>
            ) : (
                <div>
                    {comments.map(comment => (
                        <div key={comment.commentId}>
                            <p>{comment.commentText}</p>
                            <p>Published on: {comment.DateCreate}</p>
                        </div>
                    ))}
                </div>
            )}
            <Form onSubmit={handleAddComment}>
                <Form.Group controlId="formComment">
                    <Form.Label>Add a Comment</Form.Label>
                    <Form.Control
                        as="textarea"
                        rows={3}
                        placeholder="Enter your comment"
                        value={newComment}
                        onChange={(e) => setNewComment(e.target.value)}
                    />
                </Form.Group>
                <Button variant="primary" type="submit">
                    Add Comment
                </Button>
            </Form>
        </div>
    );
};

export default CommentSection;