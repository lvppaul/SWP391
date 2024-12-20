import { useState } from 'react';
import { Button, Container, Row, Col, Image, Nav, Form, InputGroup, FormControl, Spinner } from 'react-bootstrap';
import "./Signup.css";
import logo from "../../assets/Fpt_TTKoi_logo.svg";
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { signUp } from '../../Config/LogInApi';
import { BiArrowBack } from 'react-icons/bi';
import { FaEye, FaEyeSlash } from 'react-icons/fa';
import LoginGoogle from '../Login/LoginGoogle';
import ConfirmEmail from '../../components/ConfirmEmail/ConfirmEmail';
import { createCart } from '../../Config/CartApi';
import { getUserIdByEmail } from '../../Config/UserApi';

function Signup() {
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [signupError, setSignupError] = useState(null);
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const [showConfirmEmailModal, setShowConfirmEmailModal] = useState(false);
    const [loading, setLoading] = useState(false);

    const handleSignup = (e) => {
        e.preventDefault();
        setLoading(true);
        const userData = { email, password, confirmPassword, firstName, lastName };
        if (password !== confirmPassword) {
            setSignupError('Passwords do not match');
            setLoading(false);
            return;
        }

        signUp(userData)
            .then(async (response) => {
                if (response !== null) {
                    setSignupError(response);
                } else {
                    try {
                        const userId = await getUserIdByEmail(userData.email);
                        await createCart(userId);
                    } catch (error) {
                        console.error('Error create cart for user:', error);
                    }
                    setShowConfirmEmailModal(true);
                }
            })
            .catch((error) => {
                setSignupError(error.message);
            })
            .finally(() => {
                setLoading(false);
            });
    }


    return (
        <Container fluid className="signup-container vh-100">
            <Button
                variant="light"
                className="position-absolute"
                style={{
                    backgroundColor: '#E47E39',
                    top: '10px',
                    left: '0px',
                    fontSize: '20px',
                    fontWeight: 'bold',
                    width: '200px',
                    height: '50px',
                    borderTopRightRadius: '25px',
                    borderBottomRightRadius: '25px',
                    border: 'none'
                }}
                onClick={() => navigate('/')}
            >
                <BiArrowBack size={30} className="me-2" />
                Back to home
            </Button>
            <Container className='d-flex flex-row justify-content-between '>
                <Row className='d-flex justify-content-between align-items-center'>
                    <Col md={6} className='d-flex flex-column align-items-center justify-content-center me-4'>
                        <Image src={logo} alt="FPT TTKoi logo" className="signup-logo" fluid />
                        <h1 className=' fw-bold' style={{ color: "#D6691E" }}>Welcome to FPT TTKoi</h1>
                        <div className='text-dark fs-5 fw-bold text-center'>
                            <p>
                                Our website will provide the best solution to help Koi enthusiasts manage and care for your Koi fish at home.
                            </p>
                            <p>
                                We provide a range of features to monitor, track and maintain optimal conditions for Koi ponds and fish health.
                            </p>
                        </div>
                    </Col>
                    <Col md={5} className='ms-4'>
                        <Nav className='nav-tabs-login' variant="tabs" defaultActiveKey="/signup" >
                            <Nav.Item>
                                <Nav.Link eventKey="/login" href='/login'>Log In</Nav.Link>
                            </Nav.Item>
                            <Nav.Item>
                                <Nav.Link href="/signup">Sign Up</Nav.Link>
                            </Nav.Item>
                            <Nav.Item>
                                <Nav.Link eventKey="createShopAcc" href="/createshopacc">Sign Up as a Shop Owner</Nav.Link>
                            </Nav.Item>
                        </Nav>
                        <Form className='signup-form' onSubmit={handleSignup}>
                            <Row>
                                <Form.Group>
                                    <Form.Label className='signup-label'>First Name</Form.Label>
                                    <InputGroup>
                                        <FormControl
                                            type="text"
                                            placeholder="First Name"
                                            value={firstName}
                                            onChange={(e) => setFirstName(e.target.value)}
                                            required
                                            autoComplete='given-name'
                                        />
                                    </InputGroup>
                                </Form.Group>
                            </Row>
                            <Row>
                                <Form.Group>
                                    <Form.Label className='signup-label'>Last Name</Form.Label>
                                    <InputGroup>
                                        <FormControl
                                            type="text"
                                            placeholder="Last Name"
                                            value={lastName}
                                            onChange={(e) => setLastName(e.target.value)}
                                            required
                                            autoComplete='family-name'
                                        />
                                    </InputGroup>
                                </Form.Group>
                            </Row>
                            <Row>
                                <Form.Group>
                                    <Form.Label className='signup-label'>Email</Form.Label>
                                    <InputGroup>
                                        <FormControl
                                            type="email"
                                            placeholder="Email"
                                            value={email}
                                            onChange={(e) => setEmail(e.target.value)}
                                            required
                                            autoComplete='email'
                                        />
                                    </InputGroup>
                                </Form.Group>
                            </Row>
                            <Row>
                                <Form.Group>
                                    <Form.Label className='signup-label'>Password</Form.Label>
                                    <InputGroup>
                                        <FormControl
                                            type={showPassword ? "text" : "password"}
                                            placeholder="Password"
                                            value={password}
                                            onChange={(e) => setPassword(e.target.value)}
                                            required
                                            autoComplete='current-password'
                                            style={{ borderRadius: '5px' }}
                                        />
                                        <div
                                            onClick={() => setShowPassword(!showPassword)}
                                            style={{
                                                position: 'absolute',
                                                right: '10px',
                                                top: '50%',
                                                transform: 'translateY(-70%)',
                                                cursor: 'pointer',
                                                zIndex: 1,
                                            }}
                                        >
                                            {showPassword ? <FaEyeSlash /> : <FaEye />}
                                        </div>
                                    </InputGroup>
                                </Form.Group>
                                {/* <Form.Text className='d-flex justify-content-start align-items-left text-muted'>
                                    Password must be at least 6 characters long, 
                                    contain at least one uppercase letter, 
                                    one lowercase letter, one number and one special character.
                                </Form.Text> */}
                            </Row>
                            <Row className='mb-3'>
                                <Form.Group>
                                    <Form.Label className='signup-label'>Confirm Password</Form.Label>
                                    <InputGroup>
                                        <FormControl
                                            type={showConfirmPassword ? "text" : "password"}
                                            placeholder="Confirm Password"
                                            value={confirmPassword}
                                            onChange={(e) => setConfirmPassword(e.target.value)}
                                            required
                                            autoComplete='current-password'
                                            style={{ borderRadius: '5px' }}
                                        />
                                        <div
                                            onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                                            style={{
                                                position: 'absolute',
                                                right: '10px',
                                                top: '50%',
                                                transform: 'translateY(-70%)',
                                                cursor: 'pointer',
                                                zIndex: 1,
                                            }}
                                        >
                                            {showConfirmPassword ? <FaEyeSlash /> : <FaEye />}
                                        </div>
                                    </InputGroup>
                                </Form.Group>
                            </Row>
                            <Row className='already mb-3'>
                                <Col className='d-flex flex-column justify-content-start'>
                                    <p className='mb-0'>Already have an account? </p>
                                    <Link to='/login' style={{ color: '#D6691E' }}>Log in</Link>
                                </Col>
                                <Col className='d-flex justify-content-end'>
                                    <Button type='submit' className='create-button' disabled={loading}>
                                        {loading ? <Spinner animation="border" size="sm" /> : 'Create'}
                                    </Button>
                                </Col>
                            </Row>
                            {signupError && <p className="error-message">{signupError}</p>}
                            <p style={{ fontSize: "20px", fontWeight: "bold", textShadow: "black 0 0 1px" }}>Or</p>
                            <LoginGoogle />
                        </Form>
                    </Col>
                </Row>
            </Container>
            <ConfirmEmail
                show={showConfirmEmailModal}
                handleClose={() => setShowConfirmEmailModal(false)}
                email={email}
            />
        </Container >
    );
};

export default Signup;