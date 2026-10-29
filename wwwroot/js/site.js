document.addEventListener('DOMContentLoaded', () => {
  const input = document.getElementById('file-input');
  const label = document.getElementById('file-name');
  if (input && label) input.addEventListener('change', () => {
    label.textContent = input.files.length ? input.files[0].name : 'TXT, MD, CS, JS, JSON and more · up to 2 MB';
  });
});
