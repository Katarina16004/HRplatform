import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import SkillsPage from './pages/SkillsPage';
import { Toaster } from 'react-hot-toast';

const App = () => {
    return (
        <Router>
            <div style={{ backgroundColor: '#9fcdfe', boxShadow: '0 2px 20px rgb(130, 156, 242)' }}>
            <Toaster position="top-center" />
                <nav style={{ 
                    padding: '1rem 2rem', 
                    backgroundColor: '#f8f9fa', 
                    borderBottom: '1px solid #ddd',
                    display: 'flex',
                    gap: '20px' 
                }}>
                    <Link to="/" style={{ color: '#007bff', fontWeight: 'bold' }}>Candidates</Link>
                    <Link to="/skills" style={{ color: '#007bff', fontWeight: 'bold' }}>Skills</Link>
                </nav>

                <main style={{padding: '20px' }}>
                    <Routes>
                        <Route path="/" element={<div>CandidatesPage</div>} />
                        <Route path="/skills" element={<SkillsPage />} />
                    </Routes>
                </main>
            </div>
        </Router>
    );
};

export default App;