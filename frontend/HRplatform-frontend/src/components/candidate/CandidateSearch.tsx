import { useState } from 'react';
import { candidateApi } from '../../api_services/candidate/CandidateAPIService';
import type { Candidate } from '../../models/Candidate';
import toast from 'react-hot-toast';

interface Props {
    onResultsFound: (results: Candidate[]) => void;
    onClear: () => Promise<void>;
}

const CandidateSearch = ({ onResultsFound , onClear }: Props) => {
    const [nameSearch, setNameSearch] = useState('');
    const [skillsSearch, setSkillsSearch] = useState('');

    const handleSearch = async () => {
        if(!nameSearch.trim() && !skillsSearch.trim()) {
            toast.error('Please enter at least a name or skills to search.');
            return;
        }

        const results = await candidateApi.searchCandidates(
            nameSearch.trim(), 
            skillsSearch.trim()
        );

        if (results.length > 0) {
            onResultsFound(results);
            toast.success(`Found ${results.length} candidates!`);
        } else {
            toast.error('No candidates found.');
        }
    };

    const handleClear = async () => {
        setNameSearch('');
        setSkillsSearch('');
        onClear(); 
    };

    return (
        <div style={{ marginBottom: '30px', padding: '15px' }}>
            <div style={{ display: 'flex', flexDirection: 'column', gap: '10px', alignItems: 'center' }}>
            <h2 style={{ color: 'black' }}>Search candidates</h2>
            
            
                <input 
                    type="text" 
                    value={nameSearch} 
                    onChange={(e) => setNameSearch(e.target.value)} 
                    placeholder="Enter candidate name" 
                    style={{ padding: '6px', width: '300px', borderRadius: '4px', border: '1px solid #ccc' }}
                />
                
                <input 
                    type="text" 
                    value={skillsSearch} 
                    onChange={(e) => setSkillsSearch(e.target.value)} 
                    placeholder="Enter skills" 
                    style={{ padding: '6px', width: '300px', borderRadius: '4px', border: '1px solid #ccc' }}
                />
            </div>

            <div style={{ marginTop: '15px' }}>
                <button onClick={handleSearch} style={{
                    padding: '6px 12px',
                    cursor: 'pointer',
                    fontSize: '15px',
                    border: 'none',
                    borderRadius: '4px',
                    background: '#007bff',
                    color: 'white',
                    marginRight: '10px'
                }}>
                    Search
                </button>

                <button onClick={handleClear} style={{
                    padding: '6px 12px',
                    cursor: 'pointer',
                    fontSize: '15px',
                    border: '1px solid #ccc',
                    borderRadius: '4px',
                    background: 'white',
                    color: '#333'
                }}>
                    Reset
                </button>
            </div>
        </div>
    );
};

export default CandidateSearch;